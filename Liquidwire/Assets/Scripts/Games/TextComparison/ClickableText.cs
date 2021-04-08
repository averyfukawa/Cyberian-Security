﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickableText : MonoBehaviour
{
    public TextMeshProUGUI textField = null;
    public UnderlineRender underLiner;
    public CaseFolder caseFolder;

    //Basically Enum variables
    private readonly String _linkBegin = "<link=";
    private readonly String _colorRed = "<color=red";
    
    private String[] _splitArray;
    private TMP_LinkInfo[] _splitInfo;
    private ArrayList _selected = new ArrayList();
    [SerializeField]
    private List<string> _answers = new List<string>();

    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        _mainCamera = Camera.main;
    }

    private void SetText(string newString)
    {
        GetComponent<TextMeshProUGUI>().text = newString;
    }
    private void Update()
    {
        //Check if the left mouse button was used to click
        if (Input.GetMouseButtonUp(0)) // TODO add separation for stacked papers here
        {
            //Finds the correct link based on the mouse position and the text field
            var linkIndex = TMP_TextUtilities.FindIntersectingLink(textField, Input.mousePosition, _mainCamera);
            if (linkIndex == -1)
            {
                return;
            }
            //linkInfo is an array that contains the id's and the words that match.
            _splitInfo = textField.textInfo.linkInfo;
            
            int linkId = int.Parse(_splitInfo[linkIndex].GetLinkID());
            if (!_selected.Contains(linkId))
            {
                underLiner.CreateLines(CreateUnderlineCoords(linkId), caseFolder.CurrentPageNumber(), linkId);
                _selected.Add(linkId);
            }
            else
            {
                underLiner.DestroyLine(linkId);
                _selected.Remove(linkId);
            }
        }
    }
    
    private Vector3[] CreateUnderlineCoords(int linkIndex)
    {   // this method calculates the coordinates used by the line renderer,
        // they are returned in pairs of left, right grouped in an array with a total length of (lines of text in link)*2
        List<Vector3> coords = new List<Vector3>();
        TMP_TextInfo textInfo = textField.textInfo;

        Vector3 offset = new Vector3(0,0,-.001f);
        TMP_LinkInfo linkInfo = textInfo.linkInfo[linkIndex];
        int startLine = textField.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex].lineNumber;
        int endLine = textField.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex+linkInfo.linkTextLength].lineNumber;
        Transform textTransform = textField.transform;
        for (int i = startLine; i < endLine+1; i++)
        { // TODO make this ignore things past the ends of the link text
            coords.Add(textTransform.TransformPoint(textInfo.characterInfo[textInfo.lineInfo[i].firstCharacterIndex].bottomLeft) + offset);
            coords.Add(textTransform.TransformPoint(textInfo.characterInfo[textInfo.lineInfo[i].lastCharacterIndex-3].bottomRight) + offset);
        }
        return coords.ToArray();
    }
    //This is used to check if the current piece of String is the same as the piece of string that was pressed.
    private bool CheckCurrent(TMP_LinkInfo info, String current)
    {
        String s = "";
        
        foreach (var item in current.ToCharArray())
        {
            if (Char.IsDigit(item))
            {
                s += item;
            }
        }
        
        return s.Equals(info.GetLinkID());
        // return current.Equals(_linkBegin + info.GetLinkID());
    }

    public ArrayList getSelected()
    {
        return _selected;
    }

    public TMP_LinkInfo[] getSplit()
    {
        return _splitInfo;
    }

    public void ResetSelected()
    {
        _selected = new ArrayList();
    }

    public void SetAnswers(List<string> answers)
    {
        _answers = answers;
    }

    public List<string> GetAnswers()
    {
        return _answers;
    }
}