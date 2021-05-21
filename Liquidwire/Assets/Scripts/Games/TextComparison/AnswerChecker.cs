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
            _clickableText = GetComponentsInChildren<ClickableText>();
            _imageDiscrepancy = GetComponentsInChildren<ImageDiscrepancy>();
        }

        public void GradeCase()
        { // TODO validate that all papers for the case are printed and filed (probably by not enabling the button)
            CaseGrading caseGrader = new CaseGrading();
            int difficulty = 1;  // TODO fetch the emaillisting by casenumber from the mission list, then get the difficulty from there
            GetComponent<CaseFolder>().DisplayOutcome(caseGrader.Evaluation(CheckAnswers(), difficulty));
            
        }

        /* In this method it will take the answers from the provided classes and then check to see if the answers are correct */
        private int CheckAnswers()
        {
            int correct = 0;
            int totalCount = 0;
            int selectedCount = 0;
            int answerCount = 0;
            foreach (var text in _clickableText)
            {

                List<string> answers = text.GetAnswers();
                List<int> selected = text.getSelected();
                //This is used to get the actual words
                foreach (var select in selected)
                {
                    foreach (var answer in answers)
                    {
                        if (answer.Equals(select.ToString()))
                        {
                            correct++;
                            break;
                        }
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
            int returnValue = answerCount - correct;
            returnValue += totalImageCount - imageCount;
            Debug.Log(returnValue);
            return returnValue;
        }
    }
}
