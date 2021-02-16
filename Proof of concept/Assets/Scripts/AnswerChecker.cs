using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerChecker : MonoBehaviour
{
    //these classes are provided in the unity component
    public TextCreator _textCreator;
    public ClickableText _clickableText;
    /* In this method it will take the answers from the provided classes and then */
    public void AnswerChecked()
    {
        ArrayList answers = _textCreator.getAnswers();
        ArrayList selected = _clickableText.getSelected();
        
        int correct = 0;
        foreach (var select in selected)
        {
            int temp = correct;
            foreach (var answer in answers)
            {
                if (answer.Equals(select))
                {
                    correct++;
                    print(select+": was right!");
                }
            }

            if (temp == correct)
            {
                print(select + ": was wrong!");
            }
        }
        
        if (correct == answers.Count)
        {
            print("You won!");
        }
        else
        {
            print("You lost!");
        }
    }
}
