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
        
        // textField.text = "<link=1>Hi,</link> <link=2>this</link> <link=3>is</link> <link=4>a</link> <link=5>scam</link> " +
        //                  "<link=6>sentence</link>";
        
        answers.Add("1");
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
