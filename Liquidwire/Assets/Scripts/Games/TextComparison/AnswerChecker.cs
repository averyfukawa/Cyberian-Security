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

        public void FetchAnswerable()
        {
            _clickableText = FindObjectsOfType<ClickableText>();
            _imageDiscrepancy = FindObjectsOfType<ImageDiscrepancy>();
        }

        public void GradeCase()
        {
            CaseGrading caseGrader = new CaseGrading();
            int difficulty = 1;  // TODO fetch the emaillisting by casenumber, then get the difficulty from there
            // GetComponent<CaseFolder>().DisplayOutcome(caseGrader.EvaluationTextComparison(difficulty,)); // TODO make this work
        }

        /* In this method it will take the answers from the provided classes and then check to see if the answers are correct */
        public void AnswerChecked()
        { // TODO validate that all papers for the case are printed and filed
            int correct = 0;
            int totalCount = 0;
            int selectedCount = 0;
            int answerCount = 0;
            foreach (var text in _clickableText)
            {

                List<string> answers = text.GetAnswers();
                List<int> selected = text.getSelected();
                //This is used to get the actual words
                TMP_LinkInfo[] info = text.getSplit();
                foreach (var select in selected)
                {
                    int temp = correct;
                    foreach (var answer in answers)
                    {
                        if (answer.Equals(select.ToString()))
                        {
                            correct++;
                            print(info[select].GetLinkText() + ": was right!");
                            break;
                        }
                    }
                
                    if (temp == correct)
                    {
                        print(info[select].GetLinkText() + ": was wrong!");
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
                Debug.Log("win");
            }
            else
            {
                Debug.Log("loss");
            }
        }
    }
}
