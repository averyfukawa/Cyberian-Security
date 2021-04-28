using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isScam = false;
    private bool _isSelected = false;

    /// <summary>
    /// Will invert the current visual selected indicator
    /// </summary>
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
    
    /// <summary>
    /// Checks if the answers if correct.
    /// </summary>
    /// <returns></returns>
    public bool CheckAnswer()
    {
        return isScam == _isSelected;
    }
}
