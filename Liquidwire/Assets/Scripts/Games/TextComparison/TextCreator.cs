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
        textfield = textfield.Replace("\r", " \r");
        textfield = textfield.Replace("\n", " \n");
        
        answers = new List<string>();
        _dcText = dc;
        string[] splitTrue = textfield.Split('|'); 
        string[] splitText = dc.Split('|');
        int counter = 1;

        for (int i = 0; i < splitTrue.Length; i++)
        {
            if (splitText.Length <= i)
            {
                break;
            } 
            if (!splitTrue[i].Equals(splitText[i]))
            {
                answers.Add(counter.ToString());
            }

            counter++;
        }
        textfield = textfield.Replace(" \r", "\r");
        textfield = textfield.Replace(" \n", "\n");
        
        _textFieldObject.GetComponent<ClickableText>().SetAnswers(answers);
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
              newText += "<link=" + counter + ">" + text + "</link> ";
              counter++;  
            }
        }

        return newText;
    }
}
