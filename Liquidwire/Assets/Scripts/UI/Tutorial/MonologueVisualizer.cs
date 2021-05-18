﻿using System;
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            VisualizeText("This is testing text, it used to be lorem ipsum, but turns out thats a bitch to debug with because it is essentally gibberish to the non latin speaking programmer, and thus made it tedious af to figure out where the textbreaks may have messed up and swallowed parts...");
        }
    }

    public float VisualizeText(string newText)
    {
        _text.text = newText;
        // spit into substrings for each overflow, store those as queue, needs to wait for TMP execution first though
        StartCoroutine(WaitThenSplit());
        float estimatedTime = newText.Length*.05f + Mathf.Floor(newText.Length/25f);
        return estimatedTime;
    }

    private IEnumerator WaitThenSplit()
    {
        Color originalColour = _text.color;
        _text.color = new Color(0,0,0,0);
        yield return new WaitForEndOfFrame();
        while (_text.text.Length > 0 && _text.text.Length > _text.textInfo.lineInfo[_text.textInfo.lineCount - 1].lastCharacterIndex)
        {
            string block = _text.text.Substring(0, _text.textInfo.lineCount > 2 ? _text.textInfo.lineInfo[_text.textInfo.lineCount - 2].lastCharacterIndex : _text.text.Length);
            _outstandingBlocks.Enqueue(block);
            _text.text = _text.text.Substring(block.Length).TrimStart(new char[]{' ', '\n'});
            _text.ForceMeshUpdate();
        }
        _text.color = originalColour;
        // start coroutine that while loops the length of the queue and displays those letter by letter by changing individual mesh alpha
        StartCoroutine(TypewriterStyleReveal());
    }

    private IEnumerator TypewriterStyleReveal()
    {
        Color revealColour = _text.color;
        while (_outstandingBlocks.Count > 0)
        {
            _text.text = _outstandingBlocks.Dequeue();
            _text.color = new Color(0,0,0,0);
            _text.ForceMeshUpdate();
            int currentlyRevealed = 0;
            while (currentlyRevealed < _text.text.Length)
            {
                int meshIndex = _text.textInfo.characterInfo[currentlyRevealed].materialReferenceIndex;
                int vertexIndex = _text.textInfo.characterInfo[currentlyRevealed].vertexIndex;
                Color32[] vertexColors = _text.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = revealColour;
                vertexColors[vertexIndex + 1] = revealColour;
                vertexColors[vertexIndex + 2] = revealColour;
                vertexColors[vertexIndex + 3] = revealColour;
                currentlyRevealed++;
                _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                yield return new WaitForSeconds(.05f);
            }
            yield return new WaitForSeconds(1f);
        }

        _text.text = "";
    }
}
