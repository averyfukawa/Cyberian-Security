using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    [SerializeField] private GameObject crosshairUI;
    public float mouseSens = 300f;
    public Transform playerBody;
    private bool _locked;

    private float _xRotation = 0f;


    public void SetCursorLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshairUI.SetActive(true);
        _locked = true;
        mouseSens = 300f;
    }

    public void SetCursorNone()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshairUI.SetActive(false);
        _locked = false;
        mouseSens = 0f;
    }

    public bool GetLockedState()
    {
        return _locked;
    }

    // Update is called once per frame
    void Update()
    {
        // update camera rotation each frame based on player rotation and mouse position
        if (_locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            playerBody.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f); 
        }
    }
}