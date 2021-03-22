using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImageDiscrepancy : MonoBehaviour
{
    public bool isScam = false;
    private bool _isSelected = false;

    public void ChangeSelected()
    {
        TextMeshProUGUI child = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _isSelected = !_isSelected;
        if (_isSelected)
        {
            child.color = Color.red;
        }
        else 
        {
            child.color = Color.white;
        }
    }


    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawCube(transform.position, GetComponent<RectTransform>().sizeDelta);
    // }
}
