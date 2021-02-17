using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCreator : MonoBehaviour
{
    public ArrayList answers = new ArrayList();

    public GameObject trueTextFieldObject;
    public GameObject textFieldObject;

    public string textfield;

    public string trueTextField;
    // Start is called before the first frame update
    void Start()
    {
        trueTextFieldObject = GameObject.Find("TrueText");
        textFieldObject = GameObject.Find("ClickableText");
    }
    
    public void StartSentence()
    {
        
        var trueTextField = trueTextFieldObject.GetComponent<TextMeshProUGUI>();
        var textField = textFieldObject.GetComponent<TextMeshProUGUI>();

        trueTextField.text = this.trueTextField;

        textField.text = HtmlIfyString(textfield);

        string[] splitTrue = this.trueTextField.Split(' '); 
        string[] splitText = textfield.Split(' ');
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
        
    }

    public String Scramble(String sentence)
    {
        return sentence;
    }
    
    public ArrayList getAnswers()
    {
        return answers;
    }

    public String HtmlIfyString(string original)
    {
        Debug.Log("Entering htmlify");
        int counter = 1;
        string[] ogSplit = original.Split(' ');
        Debug.Log("OGsplit " + ogSplit);
        string newText = "";

        foreach (var text in ogSplit)
        {
            newText += "<link=" + counter + ">" + text + "</link> ";
            counter++;

        }
        
        
        return newText;
    }
}
