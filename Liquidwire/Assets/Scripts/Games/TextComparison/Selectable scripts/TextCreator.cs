using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Games.TextComparison.Selectable_scripts
{
    [RequireComponent(typeof(ClickableText))]
    public class TextCreator : MonoBehaviour
    {
        public List<string> answers;
        private TextMeshProUGUI _textFieldTMP;
        public ClickableText clickText;
        public GameObject textFieldObject;

        [TextArea(3, 10)] public string textfield;

        [TextArea(3, 10)] public string discrepancyField;

        [UnityEngine.Range(1, 10)] public int difficulty;

        public bool discrepancyImage;

        private string _dcText;

        private void Start()
        {
            clickText = GetComponent<ClickableText>();
            clickText.enabled = false;
        }

        /// <summary>
        /// This function will start the discrepancy creation process
        /// </summary>
        public void StartSentence()
        {
            Debug.Log("This function is temporarily under construction");
            // TODO redo this in a way that does not set it over things
            /*var imageDiscrep = FindObjectOfType<ImageDiscrepancyGenerator>();
        imageDiscrep.StartUp();
        
        _textFieldTMP = textFieldObject.GetComponent<TextMeshProUGUI>();
        var clickableText = FindObjectOfType<ClickableText>();
        var imageDiscrepancy = FindObjectOfType<ImageDiscrepancy>();
        imageDiscrepancy.ResetSelected();
        // originele text

        textfield = textfield.Replace("\r", " \r");
        textfield = textfield.Replace("\n", " \n");
        if (discrepancyImage)
        {
            imageDiscrep.GenerateDiscrepancy(difficulty);
        }
        _dcText = gameObject.GetComponent<DiscrepancyGenerator>().DiscrepancyMessage(textfield, difficulty);
        discrepancyField = _dcText;
        _textFieldTMP.text = HtmlIfyString(_dcText);
        textfield = textfield.Replace(" \r", "\r");
        textfield = textfield.Replace(" \n", "\n");
        PrefabUtility.RecordPrefabInstancePropertyModifications(textFieldObject);*/
        }

        /// <summary>
        /// Set the text based on the linkId provided
        /// </summary>
        /// <param name="startingCountForLinkID"></param>
        public void SetText(int startingCountForLinkID)
        {
            _textFieldTMP = textFieldObject.GetComponent<TextMeshProUGUI>();
            _textFieldTMP.text = HtmlIfyString(discrepancyField, startingCountForLinkID);
            _textFieldTMP.ForceMeshUpdate();
        }

        /// <summary>
        /// Set the answers based on the changed text provided. 
        /// </summary>
        /// <param name="dc"></param>
        public void SetAnswers(string dc)
        {
            Undo.RecordObject(this, "Saved new Answers");
            ClickableText clickText = textFieldObject.GetComponent<ClickableText>();
            Undo.RecordObject(clickText, "Saved new Answers");
            dc = dc.Replace("\r", "");
            textfield = textfield.Replace("\r", "");

            answers = new List<string>();
            _dcText = dc;
            string[] splitTrue = textfield.Split('|');
            string[] splitText = dc.Split('|');
            int counter = 0;

            for (int i = 0; i < splitTrue.Length; i++)
            {
                if (splitText.Length <= i)
                {
                    break;
                }

                if (splitTrue[i] != splitText[i])
                {
                    answers.Add(counter.ToString());
                    Debug.Log("Added Discrepancy:");
                    Debug.Log(splitText[i]);
                    Debug.Log(splitTrue[i]);
                    Debug.Log("###################################################");
                }

                counter++;
            }

            clickText.SetAnswers(answers);
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            PrefabUtility.RecordPrefabInstancePropertyModifications(clickText);
        }

        public List<string> getAnswers()
        {
            return answers;
        }

        /// <summary>
        /// Adds the links to the string provided, the int is at what number to start the links.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="startCount"></param>
        /// <returns></returns>
        public String HtmlIfyString(string original, int startCount)
        {
            int counter = startCount;
            string[] ogSplit = original.Split('|');
            string newText = "";

            foreach (var text in ogSplit)
            {
                if (!String.IsNullOrEmpty(text))
                {
                    string textValue = text;
                    while (textValue.StartsWith("\n"))
                    {
                        newText += "\n";
                        textValue = textValue.Remove(0, 1);
                    }

                    newText += "<link=" + counter + ">" + textValue + "</link> ";
                    counter++;
                }
            }

            return newText;
        }
    }
}