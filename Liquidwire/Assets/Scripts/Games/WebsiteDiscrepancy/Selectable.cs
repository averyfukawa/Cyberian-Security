using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isScam = false;
    private bool _isSelected = false;

    public void ChangeSelected()
    {
        TextMeshProUGUI child = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _isSelected = !_isSelected;
        if (_isSelected)
        {
            child.color = Color.blue;
        }
        else 
        {
            child.color = Color.black;
        }
    }
    
    public bool CheckAnswer()
    {
        return isScam == _isSelected;
    }
}
