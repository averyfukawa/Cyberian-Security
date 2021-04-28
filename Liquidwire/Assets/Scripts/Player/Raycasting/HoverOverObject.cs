using System.Collections;
using System.Collections.Generic;
using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverObject : MonoBehaviour
{
    public float theDistance;
    public float maxDistance;
    private static GameObject _textField;
    private static GameObject _player;
    private bool _isPlaying = false;
    [SerializeField] private bool _isPickup = true;
    [SerializeField] private bool _isHelpNotes;
    [Range(-.3f, .3f)][SerializeField] private float _distanceAdjustment; // this value is used to adjust the distance of a given object to be closer, or further
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private bool _isActive = true;
    public bool flipIt = false;
    public bool isInspection = false;
    public virtual void Start()
    {
        if (_textField == null)
        {
            _textField = GameObject.FindGameObjectWithTag("HoverText");
            _player = GameObject.FindGameObjectWithTag("GameController");
        }
        SetOriginPoints();
    }

    // Update is called once per frame
    void Update()
    {
        theDistance = RayCasting.distanceTarget;
    }

    public void ToggleActive()
    {
        _isActive = !_isActive;
    }

    public virtual void OnMouseOver()
    {
        if (!CameraMover.Instance.isMoving && _isActive)
        {
            // move into the screen view mode
            if (theDistance < maxDistance && !_isPlaying && !PlayerData.Instance.isInViewMode)
            {
                _textField.SetActive(true);

                if (Input.GetButtonDown("Action"))
                {
                    _textField.SetActive(false);
                    _player = GameObject.FindGameObjectWithTag("GameController");

                    if (!_isPickup)
                    {
                        CameraMover.Instance.MoveCameraToPosition((int) PositionIndexes.InFrontOfMonitor, 1.5f);
                        StartCoroutine(
                            SetupVCAfterWait(
                                1.5f)); // sets up the virtual canvas which is a necessity due to a b-ug with TMP
                    }
                    else
                    {
                        CameraMover.Instance.MoveObjectToPosition((int) PositionIndexes.InFrontOfCamera,
                            1f, gameObject, _distanceAdjustment, flipIt, isInspection);
                    }

                    _player.GetComponent<Movement>().ChangeLock();
                    _isPlaying = true;
                    PlayerData.Instance.isInViewMode = true;
                }
            }
            else if (theDistance > maxDistance && _textField.activeSelf)
            {
                _textField.SetActive(false);
            }
            // move out of the screen view mode
            else if (_isPlaying)
            {
                if (Input.GetButtonDown("Cancel") && !Input.GetButtonDown("Action"))
                {
                    _player = GameObject.FindGameObjectWithTag("GameController");

                    if (!_isPickup)
                    {
                        ReturnFromScreen();
                    }
                    else
                    {
                        if (TryGetComponent(out HelpFolder folder))
                        {
                            if (folder.CheckFolderMotion())
                            {
                                return;
                            }
                        }
                        ReturnObject();
                    }
                }
            }

        }
    }
    public bool GetPlaying()
    {
        return _isPlaying;
    }
    
    /// <summary>
    /// Activate the VirtualScreen after the provided time has eclipsed
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator SetupVCAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<VirtualScreenSpaceCanvaser>().ToggleCanvas();
    }

    /// <summary>
    /// Return the object to the original position.
    /// </summary>
    public void ReturnObject()
    {
        CameraMover.Instance.ReturnObjectToPosition(_originalPosition, _originalRotation, 
            1f, gameObject);
        PlayerData.Instance.isInViewMode = false;
        _textField.SetActive(true);
        _isPlaying = false;
    }
    
    /// <summary>
    /// Return the camera to the original camera position.
    /// </summary>
    public void ReturnFromScreen()
    {
        CameraMover.Instance.ReturnCameraToDefault(1.5f);
        GetComponent<VirtualScreenSpaceCanvaser>()
            .ToggleCanvas(); // sets up the virtual canvas which is a necessity due to a b-ug with TMP
        StopCoroutine("SetupVCAfterWait");
        PlayerData.Instance.isInViewMode = false;
        _textField.SetActive(true);
        _isPlaying = false;
    }
    
    /// <summary>
    /// Forces the player out of the inspection state.
    /// </summary>
    public void ForceQuitInspect()
    {
        _textField.SetActive(true);
        _isPlaying = false;
        CameraMover.Instance.ReactivateCursor();
        PlayerData.Instance.isInViewMode = false;
    }
    
    /// <summary>
    /// Set the original position and rotation
    /// </summary>
    public void SetOriginPoints()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    void OnMouseExit()
    {
        if (_textField.activeSelf)
        {
            _textField.SetActive(false);
        }
    }
}