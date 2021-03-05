using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverObject : MonoBehaviour
{
    public float theDistance;
    public float maxDistance;
    private GameObject _textField;
    private GameObject _player;
    private bool _isPlaying = false;
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
        // move into the screen view mode
        if (theDistance < maxDistance && !_isPlaying)
        {
            _textField.SetActive(true);
            _textField.GetComponent<Text>().text = "Use";

            if (Input.GetButtonDown("Action"))
            {
                _textField.SetActive(false);
                _player = GameObject.FindGameObjectWithTag("GameController");

                if (!_isPickup)
                {
                    CameraMover.instance.MoveCameraToPosition((int) PositionIndexes.InFrontOfMonitor, 1.5f);
                }
                else
                {
                    CameraMover.instance.MoveObjectToPosition((int) PositionIndexes.InFrontOfCamera,
                        1f, gameObject);
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
            if (Input.GetButtonDown("Cancel"))
            {
                _player = GameObject.FindGameObjectWithTag("GameController");

                if (!_isPickup)
                {
                    CameraMover.instance.ReturnCameraToDefault(1.5f);
                }
                else
                {
                    CameraMover.instance.ReturnObjectToPosition(_originalPosIndex, 
                                            1f, gameObject);
                }
                _textField.SetActive(true);
                _isPlaying = false;
                
            }
        }
    }

    void OnMouseExit()
    {
        if (_textField.activeSelf)
        {
            _textField.SetActive(false);
        }
    }
}