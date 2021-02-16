using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerChecker : MonoBehaviour
{
    //these classes are provided in the unity component
    public TextCreator _textCreator;
    public ClickableText _clickableText;
    
    /* In this method it will take the answers from the provided classes and then check to see if the answers are correct */
    public void AnswerChecked()
    {
        ArrayList answers = _textCreator.getAnswers();
        ArrayList selected = _clickableText.getSelected();
        
        //This is used to get the actual words
        TMP_LinkInfo[] info = _clickableText.getSplit();
        
        int correct = 0;
        foreach (var select in selected)
        {
            int temp = correct;
            foreach (var answer in answers)
            {
                if (answer.Equals(select))
                {
                    correct++;
                    //this converts the select from a string to an int so that the word can be taken from the array
                    print(info[Int32.Parse(select.ToString())-1].GetLinkText()+": was right!");
                }
            }

            if (temp == correct)
            {
                print(info[Int32.Parse(select.ToString())-1].GetLinkText() + ": was wrong!");
            }
        }
        
        //this last clause is there so that people don't just try and click every word
        if (correct == answers.Count && answers.Count == selected.Count)
        {
            print("You won!");
        }
        else
        {
            print("You lost!");
        }
    }
}
