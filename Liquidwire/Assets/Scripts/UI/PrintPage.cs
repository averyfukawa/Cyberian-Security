using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HoverOverObject), typeof(Rigidbody))]
public class PrintPage : MonoBehaviour
{
    private Rigidbody _rb;
    private HoverOverObject _hOO;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _hOO = GetComponent<HoverOverObject>();
        _hOO.enabled = false;
    }

    private void Update()
    {
        if (_rb.IsSleeping() && _rb.useGravity)
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _hOO.enabled = true;
            _hOO.SetOriginPoints();
        }
    }
}
