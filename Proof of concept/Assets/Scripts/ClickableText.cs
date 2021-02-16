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
            var array = textField.text.Split('>');
            var newText = "";

            for (int i = 0; i < array.Length; i++)
            {
                if (CheckCurrent(info, array[i]))
                {
                    if (!nextSelected)
                    {
                        newText += _colorRed + ">" + array[i] + ">" + array[i + 1] + ">" + "</color>";
                        i += 1;
                        selected.Add(info.GetLinkID());
                    }
                    else
                    {
                        newText += array[i] + ">";
                    }

                    nextSelected = false;
                }
                else if (array[i].Equals(_colorRed))
                {
                    if (!CheckCurrent(info, array[i + 1]))
                    {
                        newText += array[i] + ">";
                    }
                    else
                    {
                        array[i + 3] = "";
                        array[i] = "";
                        selected.Remove(info.GetLinkID());
                        nextSelected = true;
                    }
                }else
                {
                    if (!array[i].Equals("")) 
                    {
                        newText += array[i] + ">";
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
}