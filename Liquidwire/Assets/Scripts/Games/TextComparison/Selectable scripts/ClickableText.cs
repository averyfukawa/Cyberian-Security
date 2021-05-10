using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Games.TextComparison.Selectable_scripts
{
    public class ClickableText : MonoBehaviour
    {
        public TextMeshProUGUI textField = null;
        public UnderlineRender underLiner;
        /// <summary>
        /// The caseFolder the current class in in
        /// </summary>
        public CaseFolder caseFolder;

        private String[] _splitArray;
        /// <summary>
        /// All the info of the links in the TMP.
        /// </summary>
        private TMP_LinkInfo[] _splitInfo;
        
        /// <summary>
        /// All the selected answers selected by the user.
        /// </summary>
        private List<int> _selected = new List<int>();
        
        /// <summary>
        /// The correct answers
        /// </summary>
        [SerializeField]
        private List<string> answers = new List<string>();

        private bool _isActive;

        private Camera _mainCamera;

        private SFX _soundUnderline;
    
        // Start is called before the first frame update
        void Start()
        {
            textField = GetComponent<TextMeshProUGUI>();
            _mainCamera = Camera.main;
        
            _soundUnderline = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFX>();
        }
    
        private void Update()
        {
            //Check if the left mouse button was used to click
            if (Input.GetMouseButtonUp(0) && _isActive)
            {
                //Finds the correct link based on the mouse position and the text field
                var linkIndex = TMP_TextUtilities.FindIntersectingLink(textField, Input.mousePosition, _mainCamera);
                if (linkIndex == -1)
                {
                    return;
                }
                //linkInfo is an array that contains the id's and the words that match.
                _splitInfo = textField.textInfo.linkInfo;
            
                int linkId = int.Parse(_splitInfo[linkIndex].GetLinkID());
                if (!_selected.Contains(linkId))
                {
                    underLiner.CreateLines(CreateUnderlineCoords(linkIndex), caseFolder.CurrentPageNumber(), linkId);
                    _selected.Add(linkId);
                }
                else
                {
                    underLiner.DestroyLine(linkId);
                    _selected.Remove(linkId);
                }
            }
        }

        #region Create lines

        /// <summary>
        /// This will create a line underneath the link with the provided linkId
        /// </summary>
        /// <param name="linkIndex"></param>
        /// <returns></returns>
        private Vector3[] CreateUnderlineCoords(int linkIndex)
        {   // this method calculates the coordinates used by the line renderer,
            // they are returned in pairs of left, right grouped in an array with a total length of (lines of text in link)*2
            List<Vector3> coords = new List<Vector3>();
            TMP_TextInfo textInfo = textField.textInfo;

            Vector3 offset = new Vector3(0,0,-.001f);
            TMP_LinkInfo linkInfo = textInfo.linkInfo[linkIndex];
            int startLine = textField.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex].lineNumber;
            int endLine = textField.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex+linkInfo.linkTextLength].lineNumber;
            Transform textTransform = textField.transform;
            int linkStartIndex = linkInfo.linkTextfirstCharacterIndex;
            int linkEndIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength;
            for (int i = startLine; i < endLine+1; i++)
            { // sneaking suspicion that the counting is off here again TODO fix that if it is
                int startIndex = textInfo.lineInfo[i].firstCharacterIndex > linkStartIndex ? textInfo.lineInfo[i].firstCharacterIndex : linkStartIndex;
                int endIndex = textInfo.lineInfo[i].lastCharacterIndex-3 < linkEndIndex
                    ? textInfo.lineInfo[i].lastCharacterIndex-3 : linkEndIndex;
                coords.Add(textTransform.TransformPoint(textInfo.characterInfo[startIndex].bottomLeft) + offset);
                coords.Add(textTransform.TransformPoint(textInfo.characterInfo[endIndex].bottomRight) + offset);
            }
            return coords.ToArray();
        }
    
        /// <summary>
        /// Create a line underneath all the selected text.
        /// </summary>
        public void SetActive()
        {
            _isActive = true;
            foreach (var select in _selected)
            {
                underLiner.CreateLines(CreateUnderlineCoords(select), caseFolder.CurrentPageNumber(), select);
            }
        }

        #endregion

        #region Setters

        private void SetText(string newString)
        {
            GetComponent<TextMeshProUGUI>().text = newString;
        }
        public void SetInactive()
        {
            _isActive = false;
        }

        public void SetAnswers(List<string> answers)
        {
            this.answers = answers;
        }
        #endregion

        #region Getters

        public List<string> GetAnswers()
        {
            return answers;
        }
    
        public List<int> GetSelected()
        {
            return _selected;
        }

        public TMP_LinkInfo[] GetSplit()
        {
            return _splitInfo;
        }

        #endregion
    }
}