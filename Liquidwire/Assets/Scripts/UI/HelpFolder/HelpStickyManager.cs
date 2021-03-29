using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class HelpStickyManager : MonoBehaviour, IPointerClickHandler
{
    public List<HelpStickyObject> objectListByID;

    private int _currentSticky = 0;
    private Camera _mainCamera;
    private bool _isActive = false;
    [SerializeField] private Transform[] stickyPositions;
    [SerializeField] private TextMeshProUGUI _helpTextUI;
    [SerializeField] private GameObject _stickyPrefab;
    [SerializeField] private UnderlineRender _underLiner;

    private void Start()
    {
        _helpTextUI.text = CreateHelpText();
        _mainCamera = Camera.main;
        _underLiner.Setup(_helpTextUI.textInfo.pageInfo.Length);
    }

    public void ToggleInteractable()
    {
        _isActive = !_isActive;
        if (_isActive == false)
        {
            _underLiner.DropLines();
        }
        else
        {
            foreach (var obje in objectListByID)
            {
                if (obje.isStickied)
                {
                    int linkId = -1;
                    TMP_LinkInfo[] linkInfo = _helpTextUI.textInfo.linkInfo;
                    int pageNumber = -1;
                    for (int i = 0; i < linkInfo.Length; i++)
                    {
                        if (linkInfo[i].GetLinkText().Equals(obje.helpText))
                        {
                            linkId = i;
                            break;
                        }
                    }

                    for (int i = 0; i < _helpTextUI.textInfo.pageInfo.Length; i++)
                    {
                        if (linkInfo[linkId].linkTextfirstCharacterIndex + linkInfo[linkId].linkTextLength <
                            _helpTextUI.textInfo.pageInfo[i].lastCharacterIndex)
                        {
                            Debug.Log("link end for link number " + linkId + " is at " + (linkInfo[linkId].linkTextfirstCharacterIndex + linkInfo[linkId].linkTextLength));
                            Debug.Log("Page end of page number " + i + " is at " + _helpTextUI.textInfo.pageInfo[i].lastCharacterIndex);
                            pageNumber = i+1;
                            break;
                        }
                    }
                    _underLiner.CreateLines(CreateUnderlineCoords(linkId), pageNumber, linkId);
                }
            }
        }
    }

    private string CreateHelpText()
    {
        // borrowed courtesy of the textCreator script
        int counter = 0;
        string newText = "";

        foreach (var obje in objectListByID)
        {
            newText += "<link=" + counter + ">" + obje.helpText + "</link> " + "\n \n ";
            counter++;

        }

        return newText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Check if the left mouse button was used to click
        if (eventData.button == PointerEventData.InputButton.Left && _isActive)
        {
            //Finds the correct link based on the mouse position and the text field
            int linkId = TMP_TextUtilities.FindIntersectingLink(_helpTextUI, Input.mousePosition, _mainCamera);
            if (linkId != -1)
            {
                HelpStickyObject currentObj = objectListByID[linkId];
                // reference it against the sticky list to see if it should get stickied or it should get un-stickied
                if (currentObj.isStickied)
                {
                    // un-sticky it
                    currentObj.stickyNote.SetActive(false);
                    currentObj.isStickied = false;
                }
                else
                {
                    // sticky it
                    if (currentObj.stickyNote != null)
                    {
                        currentObj.stickyNote.SetActive(true);
                        currentObj.isStickied = true;
                    }
                    else
                    { // create a new sticky
                        if (_currentSticky == 10)
                        {
                            _currentSticky = 0;
                        }

                        foreach (var obje in objectListByID)
                        {
                            if (obje.stickyID == _currentSticky)
                            {
                                Destroy(obje.stickyNote);
                                obje.stickyID = -1;
                            }
                        }
                        currentObj.stickyNote = Instantiate(_stickyPrefab, stickyPositions[_currentSticky]);
                        currentObj.stickyNote.GetComponentInChildren<TextMeshProUGUI>().text = currentObj.stickyText;
                        currentObj.stickyID = _currentSticky;
                        _currentSticky++;
                        currentObj.isStickied = true;
                    }
                }

                if (currentObj.isStickied)
                {
                    _underLiner.CreateLines(CreateUnderlineCoords(linkId), _helpTextUI.pageToDisplay, linkId);
                }
                else
                {
                    _underLiner.DestroyLine(linkId);
                }
            }
        }
    }

    private Vector3[] CreateUnderlineCoords(int linkID)
    { // this method calculates the coordinates used by the line renderer,
      // they are returned in pairs of left, right grouped in an array with a total length of (lines of text in link)*2
        List<Vector3> coords = new List<Vector3>();
        TMP_TextInfo textInfo = _helpTextUI.textInfo;

        Vector3 offset = new Vector3(0,-.001f,0);
        TMP_LinkInfo linkInfo = _helpTextUI.textInfo.linkInfo[linkID];
        int startLine = _helpTextUI.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex].lineNumber;
        int endLine = _helpTextUI.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex+linkInfo.linkTextLength].lineNumber;
        Transform textTransform = _helpTextUI.transform;
        for (int i = startLine; i < endLine+1; i++)
        {
            coords.Add(textTransform.TransformPoint(textInfo.characterInfo[textInfo.lineInfo[i].firstCharacterIndex].bottomLeft) + offset);
            coords.Add(textTransform.TransformPoint(textInfo.characterInfo[textInfo.lineInfo[i].lastCharacterIndex].bottomRight) + offset);
        }
        return coords.ToArray();
    }
}

[Serializable] public class HelpStickyObject
{
    public string helpText;
    public string stickyText;
    public bool isStickied;
    public GameObject stickyNote;
    public int stickyID = -1;
}

[CustomEditor(typeof(HelpStickyManager))]
public class StickyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Add Help Object"))
        {
            HelpStickyManager stickyManager = target as HelpStickyManager;
            stickyManager.objectListByID.Add(new HelpStickyObject());
        }
    }
}
