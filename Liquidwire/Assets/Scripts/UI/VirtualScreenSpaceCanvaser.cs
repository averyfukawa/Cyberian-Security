using System;
using System.Collections;
using System.Collections.Generic;
using UI.Browser.Tabs;
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
    private GameObject _monitor;
    private Camera _monitorCam;

    private void Start()
    {
        _mainCamera = Camera.main;
        _windowRect = _window.GetComponent<RectTransform>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = _virtualCamera;
        overlayTextures.SetActive(false);
        _gRayCaster = _canvas.GetComponent<GraphicRaycaster>();
        _gRayCaster.enabled = false;
        _monitor = GameObject.FindGameObjectWithTag("VSCMonitor");
        _monitorCam = _monitor.GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// Toggles the canvas to the opposite of what it currently is.
    /// </summary>
    public void ToggleCanvas()
    {
        if (_canvas.worldCamera.Equals(_virtualCamera))
        {
            _canvas.worldCamera = _mainCamera;
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            RectTransform monRect = _monitor.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            monRect.GetWorldCorners(corners);

        
        
            float left = _monitorCam.WorldToScreenPoint(corners[0]).x;
            float right = Screen.width - _monitorCam.WorldToScreenPoint(corners[1]).x;
            float top = Screen.height - _monitorCam.WorldToScreenPoint(corners[1]).y;
            float bottom = _monitorCam.WorldToScreenPoint(corners[2]).y;
        
            RectTransform tRect = _trueRoot.GetComponent<RectTransform>();
            RectTransform pT = tRect.parent.GetComponent<RectTransform>();
            Vector2 newAnchorsMin = new Vector2(left / pT.rect.width, top / pT.rect.height);
            Vector2 newAnchorsMax = new Vector2(right / pT.rect.width, bottom / pT.rect.height);
    
            tRect.anchorMin = newAnchorsMin;
            tRect.anchorMax = newAnchorsMax;
            tRect.offsetMin = tRect.offsetMax = new Vector2(0, 0);
            
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
