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
    [SerializeField] private Transform[] stickyPositions;
    [SerializeField] private TextMeshProUGUI _helpTextUI;
    [SerializeField] private GameObject _stickyPrefab;
    [SerializeField] private UnderlineRender _underLiner;

    private void Start()
    {
        _helpTextUI.text = CreateHelpText();
        _mainCamera = Camera.main;
        _underLiner.Setup(_helpTextUI.textInfo.pageCount);
    }

    private string CreateHelpText()
    {
        // borrowed courtesy of the textCreator script
        int counter = 0;
        string newText = "";

        foreach (var obje in objectListByID)
        {
            newText += "<link=" + counter + ">" + obje.helpText + "</link>" + "\n\n";
            counter++;

        }

        return newText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Check if the left mouse button was used to click
        if (eventData.button == PointerEventData.InputButton.Left)
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
            }
        }
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
