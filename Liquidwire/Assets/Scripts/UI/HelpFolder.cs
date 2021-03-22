using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpFolder : MonoBehaviour
{
    [SerializeField] private Transform _topFlap;
    [SerializeField] private HelpPageViewer _helpViewer;
    [SerializeField] private float _openingSpeed = 1;
    [SerializeField] private float _rotationAmount;
    [SerializeField] private Vector3 _folderFlapDefaultRot;
    private bool _isOpen;

    private void Start()
    {
        // _folderFlapDefaultRot = _topFlap.rotation.eulerAngles; // TODO something spooky is going on here, pls fix
    }

    public void ToggleOpen()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            for (var i = 0; i < 2; i++)
            {
                _topFlap.LeanRotateAroundLocal(Vector3.right, _rotationAmount, _openingSpeed);
            }
        }
        else
        {
            for (var i = 0; i < 2; i++)
            {
                _topFlap.LeanRotate(_folderFlapDefaultRot, _openingSpeed/2);
            }
        }
        _helpViewer.ToggleButtons(_isOpen);
    }
}
