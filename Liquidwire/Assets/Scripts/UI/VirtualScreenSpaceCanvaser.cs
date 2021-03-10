﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualScreenSpaceCanvaser : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _placeHolderImage;
    [SerializeField] private Camera _virtualCamera;
    [SerializeField] private Transform _virtualRoot;
    [SerializeField] private Transform _trueRoot;
    [SerializeField] private Transform _window;
    private RectTransform _windowRect;
    private Camera _mainCamera;
    

    private void Start()
    {
        _mainCamera = Camera.main;
        _windowRect = _window.GetComponent<RectTransform>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = _virtualCamera;
    }

    public void ToggleCanvas()
    {
        if (_canvas.worldCamera.Equals(_virtualCamera))
        {
            _canvas.worldCamera = _mainCamera;
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _window.SetParent(_trueRoot, false);
            _windowRect.SetAll(0);
        }
        else
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = _virtualCamera;
            _window.SetParent(_virtualRoot, false);
            _windowRect.SetAll(0);
        }
        _placeHolderImage.SetActive(!_placeHolderImage.activeSelf);
    }
}
