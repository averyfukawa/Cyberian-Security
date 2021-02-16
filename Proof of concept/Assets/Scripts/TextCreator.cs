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
        trueTextField.text = "<link=1>Hello,</link> <link=2>this</link> <link=3>is</link> <link=4>a</link> <link=5>scam</link>" +
                             " <link=6>sentence</link>";
        textField.text = "<link=1>Hi,</link> <link=2>this</link> <link=3>is</link> <link=4>a</link> <link=5>scam</link> " +
                         "<link=6>sentence</link>";
        
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
}
