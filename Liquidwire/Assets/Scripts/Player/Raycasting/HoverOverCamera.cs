using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverCamera : HoverOverObject
{
    [SerializeField] private Transform _targetPoint;
    private static Transform _startPoint;
    private GameObject _player;
    private static Camera _camera;
    private bool _playing = false;

    public override void Start()
    {
        textField = GameObject.FindGameObjectWithTag("HoverText");
        if (_camera == null)
        {
            _camera = Camera.main;
            _startPoint = GameObject.FindGameObjectWithTag("DefaultCameraPos").transform;
        }
    }

    public override void OnMouseOver()
    {
        if (theDistance < maxDistance && !_playing)
        {
            textField.SetActive(true);
            textField.GetComponent<Text>().text = "Use";

            if (Input.GetButtonDown("Action"))
            {
                textField.SetActive(false);
                _player = GameObject.FindGameObjectWithTag("GameController");
                _camera.GetComponent<MouseCamera>().setCursorNone();

                _camera.transform.LeanRotate(_targetPoint.eulerAngles, 1.5f);
                _camera.transform.LeanMove(_targetPoint.position, 1.5f);
            
                _player.GetComponent<Movement>().changeLock();
                _playing = true;
            }
        }
        else if (theDistance > maxDistance && textField.activeSelf)
        {
            textField.SetActive(false);
        }
        else if (_playing)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                _camera.transform.LeanRotate(_startPoint.eulerAngles, 1.5f);
                _camera.transform.LeanMove(_startPoint.position, 1.5f);

                _player = GameObject.FindGameObjectWithTag("GameController");
                _camera.GetComponent<MouseCamera>().setCursorLocked();

                textField.SetActive(true);
                _player.GetComponent<Movement>().changeLock();
                _playing = false;
                
            }
        }
    }
}