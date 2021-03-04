using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverCamera : HoverOverObject
{
    
    private GameObject _player;
    private bool _isPlaying = false;

    public override void Start()
    {
        textField = GameObject.FindGameObjectWithTag("HoverText");
    }
    
    public override void OnMouseOver()
    {
        // move into the screen view mode
        if (theDistance < maxDistance && !_isPlaying)
        {
            textField.SetActive(true);
            textField.GetComponent<Text>().text = "Use";

            if (Input.GetButtonDown("Action"))
            {
                textField.SetActive(false);
                _player = GameObject.FindGameObjectWithTag("GameController");
                CameraMover.instance.MoveCameraToPosition(0, 1.5f);
                
                _player.GetComponent<Movement>().changeLock();
                _isPlaying = true;
            }
        }
        else if (theDistance > maxDistance && textField.activeSelf)
        {
            textField.SetActive(false);
        }
        // move out of the screen view mode
        else if (_isPlaying)
        {
            if (Input.GetButtonDown("Cancel"))
            {

                _player = GameObject.FindGameObjectWithTag("GameController");
                CameraMover.instance.ReturnCameraToDefault(1.5f);

                textField.SetActive(true);
                _player.GetComponent<Movement>().changeLock();
                _isPlaying = false;
                
            }
        }
    }
}