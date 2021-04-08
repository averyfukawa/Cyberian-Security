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
    private readonly String _colorRed = "<color=red";
    
    private String[] _splitArray;
    private TMP_LinkInfo[] _splitInfo;
    private ArrayList _selected = new ArrayList();
    [SerializeField]
    private List<string> _answers = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
    }

    private void SetText(string newString)
    {
        GetComponent<TextMeshProUGUI>().text = newString;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Check if the left mouse button was used to click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Finds the correct link based on the mouse position and the text field
            var linkId = TMP_TextUtilities.FindIntersectingLink(textField, Input.mousePosition, null);
            if (linkId == -1)
            {
                return;
            }
            //linkInfo is an array that contains the id's and the words that match.
            _splitInfo = textField.textInfo.linkInfo;
            var info = _splitInfo[linkId];

            var wasUnselected = false;
            
            /*It splits the text in the textField for every '>' there is. It does this so that it can edit the text to
             include color so that the user knows tat it has been clicked */
            _splitArray = textField.text.Split('>');
            var newText = "";

            for (int i = 0; i < _splitArray.Length; i++)
            {
                
                
                if (i != _splitArray.Length - 1)
                {
                    if (CheckCurrent(info, _splitArray[i]))
                    {
                        //If the word was unselected it wouldn't be capable of going in.
                        if (!wasUnselected)
                        {
                            // Adds the <color=red> </color> around the pressed word
                            // newText += _colorRed + ">" + _splitArray[i] + ">" + _splitArray[i + 1] + ">" + "</color>";
                            //TODO instead lines the selected objects
                            

                            //this makes it skip the next String, since it already been added above here
                            i += 1;
                            _selected.Add(info.GetLinkID());
                        }
                        else
                        {
                            newText += _splitArray[i] + ">";
                        }

                        wasUnselected = false;
                    }
                    //if the current String in the array is <color=red it will go into this else if.
                    else if (_splitArray[i].Equals(_colorRed))
                    {
                        //If the next String in the array isn't the one that needs to be found then it can enter the if.
                        if (!CheckCurrent(info, _splitArray[i + 1]))
                        {
                            newText += _splitArray[i] + ">";
                        }
                        else
                        {
                            //This else is here to remove the color from unselected words. 
                            _splitArray[i + 3] = "";
                            _splitArray[i] = "";
                            _selected.Remove(info.GetLinkID());
                            //this is here to make sure it doesn't set the color there again 
                            wasUnselected = true;
                        }
                    }
                    else
                    {
                        //if the current String in the array isn't empty it will add it to the text and add a '>' to it.
                        if (!_splitArray[i].Equals(""))
                        {
                            newText += _splitArray[i] + ">";
                        }
                    }
                }
            }

            textField.text = newText;
        }
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