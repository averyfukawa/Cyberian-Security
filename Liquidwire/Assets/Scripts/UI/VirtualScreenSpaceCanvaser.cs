using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualScreenSpaceCanvaser : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _placeHolderImage;
    [SerializeField] private Camera _virtualCamera;
    [SerializeField] private Transform _virtualRoot;
    [SerializeField] private Transform _trueRoot;
    [SerializeField] private Transform _window;
    public GameObject overlayTextures;
    private RectTransform _windowRect;
    private Camera _mainCamera;
    private GraphicRaycaster _gRayCaster;
    

    private void Start()
    {
        _mainCamera = Camera.main;
        _windowRect = _window.GetComponent<RectTransform>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = _virtualCamera;
        overlayTextures.SetActive(false);
        _gRayCaster = _canvas.GetComponent<GraphicRaycaster>();
        _gRayCaster.enabled = false;
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
        _gRayCaster.enabled = !_gRayCaster.isActiveAndEnabled;
        overlayTextures.SetActive(!overlayTextures.activeSelf);
    }
}
