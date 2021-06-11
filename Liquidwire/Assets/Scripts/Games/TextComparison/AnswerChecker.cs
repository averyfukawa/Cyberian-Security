using System.Collections.Generic;
using Games.PointsCalculation;
using Games.TextComparison.Selectable_scripts;
using TMPro;
using UnityEngine;

namespace Games.TextComparison
{
    public class AnswerChecker : MonoBehaviour
    {
        //these classes are provided in the unity component
        private TextCreator[] _textCreators;
        private ClickableText[] _clickableTexts;
        private ImageDiscrepancy[] _imageDiscrepancy;
        
        /// <summary>
        /// Finds all the ClickableText instances and all instances of ImageDiscrepancy
        /// </summary>
        public void FetchAnswerable()
        {
            _textCreators = GetComponentsInChildren<TextCreator>();
            _clickableTexts = GetComponentsInChildren<ClickableText>();
            _imageDiscrepancy = GetComponentsInChildren<ImageDiscrepancy>();
        }

        #region Grading Methods

        /// <summary>
        /// Grade the case based on the current case supplied?
        /// </summary>
        public void GradeCase()
        { // TODO validate that all papers for the case are printed and filed (probably by not enabling the button)
            CaseGrading caseGrader = new CaseGrading();
            int difficulty = 1;  // TODO fetch the emaillisting by casenumber from the mission list, then get the difficulty from there
            GetComponent<CaseFolder>().DisplayOutcome(caseGrader.Evaluation(CheckAnswers(), difficulty), false);
        }
        
        /// <summary>
        /// In this method it will take the answers from the provided classes and then check to see if the answers are
        /// correct. 
        /// </summary>
        /// <returns>Amount of errors</returns>
        private int CheckAnswers()
        {
            int correct = 0;
            int totalCount = 0;
            int selectedCount = 0;
            int answerCount = 0;
            /**
             * Look through all the active clickableTexts in the children grab the answers of each and
             * crossreference it with the selected answers per ClickableText. Then check the amount of errors
             */
            for(int i = 0; i < _clickableTexts.Length; i++)
            {

                List<string> answers = _textCreators[i].GetAnswers();
                List<int> selected = _clickableTexts[i].GetSelected();
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
                if (image.Check())
                {
                    imageCount++;
                }
                totalImageCount++;
            }
            int returnValue = answerCount - correct;
            returnValue += totalImageCount - imageCount;
            
            return returnValue;
        }

        #endregion
    }
}
