using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableText : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI textField = null;

    //Basically Enum variables
    private readonly String _linkBegin = "<link=";
    private readonly String _linkEnd = "</link>";
    private readonly String _colorRed = "<color=red>";
    
    private ArrayList selected = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var linkId = TMP_TextUtilities.FindIntersectingLink(textField, Input.mousePosition, null);
            var info = textField.textInfo.linkInfo[linkId];

            var nextSelected = false;
            var array = textField.text.Split(' ');
            var newText = "";

            for (int i = 0; i < array.Length; i++)
            {
                if (CheckCurrent(info, array[i]))
                {
                    if (!nextSelected)
                    { 
                        newText += _colorRed +" " + array[i] + " </color> ";
                        selected.Add(info.GetLinkID());
                    }
                    else
                    {
                        newText += array[i];
                    }
                    nextSelected = false;
                }
                else if (array[i].Equals(_colorRed))
                {
                    if (!CheckCurrent(info, array[i+1]))
                    {
                        newText += array[i] + " ";
                    }
                    else
                    {
                        array[i + 2] = "";
                        array[i] = "";
                        newText += array[i];
                        selected.Remove(info.GetLinkID());
                        print(selected.Count);
                        nextSelected = true;
                    }
                }
                else
                {
                    newText += array[i] + " ";
                }
            }

            textField.text = newText;
        }
    }

    private bool CheckCurrent(TMP_LinkInfo info, String current)
    {
        return current.Equals(_linkBegin + info.GetLinkID() + '>' + info.GetLinkText() + _linkEnd);
    }
}