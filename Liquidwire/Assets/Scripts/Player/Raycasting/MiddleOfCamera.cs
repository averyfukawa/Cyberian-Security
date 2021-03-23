using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleOfCamera : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = _mainCamera.ScreenToWorldPoint( 
            new Vector3(Screen.width/2, Screen.height/2, _mainCamera.nearClipPlane + 0.45f) );
        gameObject.transform.LookAt(_mainCamera.transform);
    }
}
