﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StickyNote : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastTargetMask;
    private Camera _mainCamera;
    private bool _isFaded;
    private RawImage _stickyOverlay;
    private Image _sticky;

    private void Start()
    {
        _mainCamera = Camera.main;
        _stickyOverlay = FindObjectOfType<VirtualScreenSpaceCanvaser>().overlayTextures.GetComponent<RawImage>();
        _sticky = GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (true) // TODO check if player is at the computer
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition),
                out RaycastHit hit, 10f, _raycastTargetMask))
            {
                if (hit.transform.Equals(transform) && !_isFaded)
                {
                    // fade out
                    ToggleFade();
                }
            }
            else if(_isFaded)
            {
                // fade in
                ToggleFade();
            }
        }
        
    }

    private void ToggleFade()
    {
        _isFaded = !_isFaded;
        if (_isFaded)
        {
            _stickyOverlay.color = new Color(1,1,1,.5f);
            _sticky.color = new Color(1,1,1,.5f);
        }
        else
        {
            _stickyOverlay.color = new Color(1,1,1,1f);
            _sticky.color = new Color(1,1,1,.5f);
        }
    }
}