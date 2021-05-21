using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TextComparison;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

[RequireComponent(typeof(ClickableText))]
public class TextCreator : MonoBehaviour
{
    public List<string> answers;
    private TextMeshProUGUI _textFieldTMP;
    public ClickableText clickText;
    public GameObject _textFieldObject;

    [TextArea(3,10)]
    public string textfield; 
     
    [TextArea(3,10)]
    public string discrepancyField; 
    
     [UnityEngine.Range(1, 10)]
     public  int difficulty;

     public bool discrapencyImage;
     public int textLength = 0;
     
     private string _dcText;

     private void Start()
     {
         clickText = GetComponent<ClickableText>();
         clickText.enabled = false;
     }

     public void StartSentence()
    {
        Debug.Log("This function is temporarily under construction");
        // TODO redo this in a way that does not set it over things
        /*var imageDiscrap = FindObjectOfType<ImageDiscrepancyGenerator>();
        imageDiscrap.StartUp();
        
        _textFieldTMP = _textFieldObject.GetComponent<TextMeshProUGUI>();
        var clickableText = FindObjectOfType<ClickableText>();
        var imageDiscrepancy = FindObjectOfType<ImageDiscrepancy>();
        imageDiscrepancy.ResetSelected();
        clickableText.ResetSelected();
        // originele text

        textfield = textfield.Replace("\r", " \r");
        textfield = textfield.Replace("\n", " \n");
        if (discrapencyImage)
        {
            imageDiscrap.GenerateDiscrapency(difficulty);
        }
        _dcText = gameObject.GetComponent<DiscrepanciesGenerator>().DiscrapeMessage(textfield, difficulty);
        discrepancyField = _dcText;
        _textFieldTMP.text = HtmlIfyString(_dcText);
        textfield = textfield.Replace(" \r", "\r");
        textfield = textfield.Replace(" \n", "\n");
        PrefabUtility.RecordPrefabInstancePropertyModifications(_textFieldObject);*/
    }

     public void SetText(int startingCountForLinkID)
     {
         _textFieldTMP = _textFieldObject.GetComponent<TextMeshProUGUI>();
         _textFieldTMP.text = HtmlIfyString(discrepancyField, startingCountForLinkID);
         _textFieldTMP.ForceMeshUpdate();
     }
    public void SetAnswers(string dc)
    {
        Undo.RecordObject(this, "Saved new Answers");
        ClickableText clickText = _textFieldObject.GetComponent<ClickableText>();
        Undo.RecordObject(clickText, "Saved new Answers");
        dc = dc.Replace("\r", "");
        textfield = textfield.Replace("\r", "");
        
        answers = new List<string>();
        _dcText = dc;
        string[] splitTrue = textfield.Split('|'); 
        string[] splitText = dc.Split('|');
        TextCreator[] texts = GetComponentInParent<Tab>().gameObject.GetComponentsInChildren<TextCreator>();
        int counter = 0;
        foreach (var t in texts)
        {
            if (t == this)
            {
                break;
            }
            else
            {
                counter += t.textLength;
            }
        }

        for (int i = 0; i < splitTrue.Length; i++)
        {
            if (splitText.Length <= i)
            {
                break;
            }
            
            if (splitTrue[i] != splitText[i])
            {
                answers.Add(counter.ToString());
                Debug.Log("Added Discrepancy:");
                Debug.Log(splitText[i]);
                Debug.Log(splitTrue[i]);
                Debug.Log("###################################################");
            }

            counter++;
        }

        textLength = splitTrue.Length-1;
        clickText.SetAnswers(answers);
        PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        PrefabUtility.RecordPrefabInstancePropertyModifications(clickText);
    }
    
    public List<string> getAnswers()
    {
        return answers;
    }

    public String HtmlIfyString(string original, int startCount)
    {
        int counter = startCount;
        string[] ogSplit = original.Split('|');
        string newText = "";

        foreach (var text in ogSplit)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string textValue = text;
                while (textValue.StartsWith("\n"))
                {
                    newText += "\n";
                    textValue =textValue.Remove(0, 1);
                }
                newText += "<link=" + counter + ">" + textValue + "</link> ";
                counter++;  
            }
        }

        return newText;
    }
}
