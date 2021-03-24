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
    private GameObject _textField;
    private GameObject _player;
    [SerializeField] private bool _isPlaying = false;
    [SerializeField] private bool _isPickup = true;
    [SerializeField] private int _originalPosIndex;
    public virtual void Start()
    {
        _textField = GameObject.FindGameObjectWithTag("HoverText");
        _player = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        theDistance = RayCasting.distanceTarget;
    }

    public virtual void OnMouseOver()
    {
        if (!CameraMover.instance._isMoving)
        {
            // move into the screen view mode
            if (theDistance < maxDistance && !_isPlaying)
            {
                _textField.SetActive(true);
                _textField.GetComponent<TextMeshProUGUI>().text = "Use";

                if (Input.GetButtonDown("Action"))
                {
                    _textField.SetActive(false);
                    _player = GameObject.FindGameObjectWithTag("GameController");

                    if (!_isPickup)
                    {
                        CameraMover.instance.MoveCameraToPosition((int) PositionIndexes.InFrontOfMonitor, 1.5f);
                        StartCoroutine(
                            SetupVCAfterWait(
                                1.5f)); // sets up the virtual canvas which is a necessity due to a b-ug with TMP
                    }
                    else
                    {
                        CameraMover.instance.MoveObjectToPosition((int) PositionIndexes.InFrontOfCamera,
                            1f, gameObject);
                        if (_originalPosIndex == 2)
                        {
                            // additional toggle of the help menu, always keep the delay equal to the travel time above
                            StartCoroutine(SetupHelpNotesAfterWait(1f));
                        }
                    }

                    _player.GetComponent<Movement>().changeLock();
                    _isPlaying = true;
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
                        CameraMover.instance.ReturnCameraToDefault(1.5f);
                        GetComponent<VirtualScreenSpaceCanvaser>()
                            .ToggleCanvas(); // sets up the virtual canvas which is a necessity due to a b-ug with TMP
                        StopCoroutine("SetupVCAfterWait");
                    }
                    else
                    {
                        CameraMover.instance.ReturnObjectToPosition(_originalPosIndex,
                            1f, gameObject);

                        if (_originalPosIndex == 2)
                        {
                            // additional toggle of the help menu
                            GetComponentInChildren<HelpStickyManager>().ToggleInteractable();
                            StopCoroutine("SetupHelpNotesAfterWait");
                        }
                    }
                    _textField.SetActive(true);
                    _isPlaying = false;
                }
            }

        }
    }

        IEnumerator SetupHelpNotesAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponentInChildren<HelpStickyManager>().ToggleInteractable();
    }

        IEnumerator SetupVCAfterWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<VirtualScreenSpaceCanvaser>().ToggleCanvas();
    }

    void OnMouseExit()
    {
        if (_textField.activeSelf)
        {
            _textField.SetActive(false);
        }
    }
}