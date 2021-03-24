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
    private bool _isOpen;

    public void ToggleOpen()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            _topFlap.LeanRotateAroundLocal(Vector3.right, _rotationAmount, _openingSpeed);
        }
        else
        {
            _topFlap.LeanRotateAroundLocal(Vector3.right, -_rotationAmount, _openingSpeed/2);
        }

        if (_helpViewer != null)
        {
            _helpViewer.ToggleButtons(_isOpen);
        }
    }
}
