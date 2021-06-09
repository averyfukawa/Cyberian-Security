using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardOrienter : MonoBehaviour
{
    private Camera mainCam;
    private BoxCollider _boundingBox;
    private static Plane[] _cameraFrustum;
    private static bool _checkedUpdate;
    
    void Start()
    {
        mainCam = Camera.main;
        _boundingBox = GetComponent<BoxCollider>();
    }
    
    void Update()
    {

        if (!_checkedUpdate) // ensure that this only happens once each frame regardless of instance number
        {
            _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(mainCam);
            _checkedUpdate = true;
        }

        if (GeometryUtility.TestPlanesAABB(_cameraFrustum, _boundingBox.bounds)) // check if object is inside camera frustum before turning
        {
            transform.LookAt(mainCam.transform.position);
        }
    }

    private void LateUpdate() // flag that it needs a new camera frustum update
    {
        if (_checkedUpdate)
        {
            _checkedUpdate = false;
        }
    }
}
