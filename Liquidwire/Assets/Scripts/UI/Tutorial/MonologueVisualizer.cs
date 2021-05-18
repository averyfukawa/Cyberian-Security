using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))] // functionally necessary
[RequireComponent(typeof(Canvas))] // for performance reason - to minimize the amount of canvas elements being updated
public class MonologueVisualizer : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Queue<string> _outstandingBlocks = new Queue<string>();

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        if (_text.overflowMode != TextOverflowModes.Ellipsis)
        {
            Debug.LogWarning("please change overflow mode of " + _text.name + " to ellipsis");
        }
    }

    public void VisualizeText(string newText)
    {
        _text.text = newText;
        // spit into substrings for each overflow, store those as queue
        
        // start coroutine that while loops the length of the queue and displays those letter by letter by changing individual mesh alpha
    }
}
