using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualScreenSpaceCanvaser : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _placeHolderImage;

    private void Start()
    {
        _canvas.SetActive(false);
    }

    public void ToggleCanvas()
    {
        _canvas.SetActive(!_canvas.activeSelf);
        _placeHolderImage.SetActive(!_placeHolderImage.activeSelf);
    }
}
