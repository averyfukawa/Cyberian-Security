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


    private void Start()
    {
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
        _viewCamera.transform.LeanMove(_targetPositions[positionIndex].position, executionTime);
        _viewCamera.transform.LeanRotate(_targetPositions[positionIndex].rotation.eulerAngles, executionTime);
        _mouseCam.SetCursorNone();
    }
    

    // return to original rotation and position
    public void ReturnCameraToDefault(float executionTime)
    {
        _viewCamera.transform.LeanMove(_defaultCameraPos.position, executionTime);
        _viewCamera.transform.LeanRotate(_defaultCameraPos.rotation.eulerAngles, executionTime);
        StartCoroutine(ReactivateCursorControl(executionTime));
    }

    // reestablish the connection to the cursor control at the end to avoid snapping
    private IEnumerator ReactivateCursorControl(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _mouseCam.SetCursorLocked();
    }
}
