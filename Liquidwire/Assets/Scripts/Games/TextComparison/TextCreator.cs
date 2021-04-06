using System;
using System.Collections;
using System.Collections.Generic;
using TextComparison;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class TextCreator : MonoBehaviour
{
    public ArrayList answers = new ArrayList();
    private TextMeshProUGUI textFieldTMP;
    public GameObject trueTextFieldObject;
    public GameObject textFieldObject;

    [TextArea(3,10)]
    public string textfield; 
     
    [TextArea(3,10)]
    public string discrepancyField; 
    
     [Range(1, 10)]
     public  int difficulty;

     public bool discrapencyImage;
     
     private string _dcText;
    // Start is called before the first frame update
    void Start()
    {
    }
    
    public void StartSentence()
    {
        var imageDiscrap = FindObjectOfType<ImageDiscrepancyGenerator>();
        imageDiscrap.StartUp();
        
        textFieldObject = GameObject.Find("ClickableText");
        textFieldTMP = textFieldObject.GetComponent<TextMeshProUGUI>();
        var clickableText = FindObjectOfType<ClickableText>();
        var imageDiscrepancy = FindObjectOfType<ImageDiscrepancy>();
        imageDiscrepancy.ResetSelected();
        clickableText.ResetSelected();
        answers = new ArrayList();
        // originele text

        textfield = textfield.Replace("\r", " \r");
        textfield = textfield.Replace("\n", " \n");
        if (discrapencyImage)
        {
            imageDiscrap.GenerateDiscrapency(difficulty);
        }
        _dcText = gameObject.GetComponent<DiscrepanciesGenerator>().DiscrapeMessage(textfield, difficulty);
        discrepancyField = _dcText;
        textFieldTMP.text = HtmlIfyString(_dcText);
        textfield = textfield.Replace(" \r", "\r");
        textfield = textfield.Replace(" \n", "\n");
    }

    public void SetAnswers(string dc)
    {
        textfield = textfield.Replace("\r", " \r");
        textfield = textfield.Replace("\n", " \n");
        answers = new ArrayList();
        _dcText = dc;
        string[] splitTrue = textfield.Split(' '); 
        string[] splitText = dc.Split(' ');
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
    }
    
    public ArrayList getAnswers()
    {
        return answers;
    }

    public String HtmlIfyString(string original)
    {
        int counter = 1;
        string[] ogSplit = original.Split(' ');
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
