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
    private readonly String _colorRed = "<color=red";
    private String[] splitArray;
    private ArrayList selected = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var linkId = TMP_TextUtilities.FindIntersectingLink(textField, Input.mousePosition, null);
            var info = textField.textInfo.linkInfo[linkId];
            
            var nextSelected = false;
            splitArray = textField.text.Split('>');
            var newText = "";

            for (int i = 0; i < splitArray.Length; i++)
            {
                if (CheckCurrent(info, splitArray[i]))
                {
                    if (!nextSelected)
                    {
                        newText += _colorRed + ">" + splitArray[i] + ">" + splitArray[i + 1] + ">" + "</color>";
                        i += 1;
                        selected.Add(info.GetLinkID());
                    }
                    else
                    {
                        newText += splitArray[i] + ">";
                    }

                    nextSelected = false;
                }
                else if (splitArray[i].Equals(_colorRed))
                {
                    if (!CheckCurrent(info, splitArray[i + 1]))
                    {
                        newText += splitArray[i] + ">";
                    }
                    else
                    {
                        splitArray[i + 3] = "";
                        splitArray[i] = "";
                        selected.Remove(info.GetLinkID());
                        nextSelected = true;
                    }
                }else
                {
                    if (!splitArray[i].Equals("")) 
                    {
                        newText += splitArray[i] + ">";
                    }
                }
            }

            textField.text = newText;
            foreach (var v in selected)
            {
                print(v);
            }
        }
    }

    private bool CheckCurrent(TMP_LinkInfo info, String current)
    {
        return current.Contains(_linkBegin + info.GetLinkID());
    }

    public ArrayList getSelected()
    {
        return selected;
    }

    public String[] getSplit()
    {
        return splitArray;
    }
}