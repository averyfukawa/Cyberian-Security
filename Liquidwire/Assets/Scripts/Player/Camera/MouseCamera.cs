using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    public float mouseSens = 300f;
    public Transform playerBody;
    private bool locked;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        locked = true;
    }

    public void setCursorLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        locked = true;
        mouseSens = 300f;
    }

    public void setCursorNone()
    {
        Cursor.lockState = CursorLockMode.None;
        locked = false;
        mouseSens = 0f;
    }

    public bool getLockedState()
    {
        return locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerBody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}