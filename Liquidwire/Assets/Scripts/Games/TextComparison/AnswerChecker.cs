using System;
using System.Collections;
using System.Collections.Generic;
using TextComparison;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TextComparison
{
    public class AnswerChecker : MonoBehaviour
    {
        //these classes are provided in the unity component
        private ClickableText[] _clickableText;
        private ImageDiscrepancy[] _imageDiscrepancy;

        public void Start()
        {
            _clickableText = FindObjectsOfType<ClickableText>();
            _imageDiscrepancy = FindObjectsOfType<ImageDiscrepancy>();
        }

        /* In this method it will take the answers from the provided classes and then check to see if the answers are correct */
        public void AnswerChecked()
        {
            int correct = 0;
            int totalCount = 0;
            int selectedCount = 0;
            int answerCount = 0;
            foreach (var text in _clickableText)
            {

                List<string> answers = text.GetAnswers();
                ArrayList selected = text.getSelected();
                //This is used to get the actual words
                TMP_LinkInfo[] info = text.getSplit();
                foreach (var select in selected)
                {
                    int temp = correct;
                    foreach (var answer in answers)
                    {
                        
                        if (answer.Equals(select))
                        {
                            correct++;
                            //this converts the select from a string to an int so that the word can be taken from the array
                            print(info[Int32.Parse(select.ToString()) - 1].GetLinkText() + ": was right!");
                        }
                    }

                    if (temp == correct)
                    {
                        print(info[Int32.Parse(select.ToString()) - 1].GetLinkText() + ": was wrong!");
                    }
                }

                //this last clause is there so that people don't just try and click every word
                var list = FindObjectsOfType<ImageDiscrepancy>();
                int counter = 0;
                foreach (var item in list)
                {
                    if (item.check())
                    {
                        counter++;
                    }
                }

                answerCount += answers.Count;
                selectedCount += selected.Count;
                totalCount += correct;
            }

            var imageCount = 0;
            var totalImageCount = 0;
            foreach (var image in _imageDiscrepancy)
            {
                if (image.check())
                {
                    imageCount++;
                }
                totalImageCount++;
            }
            if (correct == answerCount && answerCount == selectedCount && imageCount == totalImageCount)
            {
                print("You won!");
            }
            else
            {
                print("You lost!");
            }
        }
    }
}
