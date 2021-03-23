using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover instance;
    private MouseCamera _mouseCam;
    private Camera _viewCamera;
    [SerializeField] private Transform _defaultCameraPos;
    [SerializeField] private Transform[] _targetPositions;
    private GameObject _player;
    public bool _isMoving;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("GameController");
        _viewCamera = Camera.main;
        _mouseCam = FindObjectOfType<MouseCamera>();
        if (instance == null)
        {
            instance = this;
        }
    }

    // move the camera to a preestablished waypoint and break mouse ctrl over camera rotation
    public void MoveCameraToPosition(int positionIndex, float executionTime)
    {
        StartCoroutine(ReAllowMovement(executionTime));
        _viewCamera.transform.LeanMove(_targetPositions[positionIndex].position, executionTime);
        _viewCamera.transform.LeanRotate(_targetPositions[positionIndex].rotation.eulerAngles, executionTime);
        _mouseCam.SetCursorNone();
    }
    

    // return to original rotation and position
    public void ReturnCameraToDefault(float executionTime)
    {
        StartCoroutine(ReAllowMovement(executionTime));
        _viewCamera.transform.LeanMove(_defaultCameraPos.position, executionTime);
        _viewCamera.transform.LeanRotate(_defaultCameraPos.rotation.eulerAngles, executionTime);
        StartCoroutine(ReactivateCursorControl(executionTime));
    }
    
    // Move object to the position provided. This is used for picking it up and putting it down.
    public void MoveObjectToPosition(int positionIndex, float executionTime, GameObject movingObject)
    {
        StartCoroutine(ReAllowMovement(executionTime));
        movingObject.transform.LeanMove(_targetPositions[positionIndex].position, executionTime);
        movingObject.transform.LeanRotate(_targetPositions[positionIndex].rotation.eulerAngles, executionTime);
        _mouseCam.SetCursorNone();
        if (movingObject.TryGetComponent(out HelpFolder folder))
        {
            folder.ToggleOpen();
        }
    }
    
    //Return the Object to the original position provided
    public void ReturnObjectToPosition(int positionIndex, float executionTime, GameObject movingObject)
    {
        StartCoroutine(ReAllowMovement(executionTime));
        movingObject.transform.LeanMove(_targetPositions[positionIndex].position, executionTime);
        movingObject.transform.LeanRotate(_targetPositions[positionIndex].rotation.eulerAngles, executionTime);
        StartCoroutine(ReactivateCursorControl(executionTime));
        if (movingObject.TryGetComponent(out HelpFolder folder))
        {
            folder.ToggleOpen();
        }
    }

    // reestablish the connection to the cursor control at the end to avoid snapping
    private IEnumerator ReactivateCursorControl(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _mouseCam.SetCursorLocked();
        _player.GetComponent<Movement>().changeLock();
    }

    private IEnumerator ReAllowMovement(float waitTime)
    {
        _isMoving = true;
        yield return new WaitForSeconds(waitTime);
        _isMoving = false;
    }
}
