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
    private bool _isFaded;
    private Image _sticky;
    private TextMeshProUGUI _note;

    private void Start()
    {
        _mainCamera = Camera.main;
        _sticky = GetComponentInChildren<Image>();
        _note = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (true) // TODO check if player is at the computer
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition),
                out RaycastHit hit, 10f, _raycastTargetMask))
            {
                Debug.Log("hit !");
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
            _sticky.color = new Color(1,1,1,.5f);
            _note.alpha = .5f;
        }
        else
        {
            _sticky.color = new Color(1,1,1,1f);
            _note.alpha = 1;
        }
    }
}
