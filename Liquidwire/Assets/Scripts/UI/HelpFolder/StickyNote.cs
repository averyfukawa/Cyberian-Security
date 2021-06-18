using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StickyNote : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastTargetMask;
    private Camera _mainCamera;
    /// <summary>
    /// if the sticky note is being focused on it will fade a bit
    /// </summary>
    private bool _isFaded;
    private RawImage _stickyOverlay;
    private Image _sticky;

    private void Start()
    {
        _mainCamera = Camera.main;
        _stickyOverlay = FindObjectOfType<VirtualScreenSpaceCanvaser>().overlayTextures.GetComponent<RawImage>();
        _sticky = GetComponentInChildren<Image>();
    }

    // Code for the fade in/out of the stickies when hovered, not needed atm TODO if this is still not needed in final build, clean out the render mat in this

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
