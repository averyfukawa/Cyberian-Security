using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isScam = false;
    private bool _isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSelected()
    {
        _isSelected = !_isSelected;
    }
    
    public bool CheckAnswer()
    {
        return isScam == _isSelected;
    }
}
