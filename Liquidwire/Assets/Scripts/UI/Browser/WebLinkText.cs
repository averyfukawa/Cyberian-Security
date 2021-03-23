using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WebLinkText : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI _sourceText;
    private Coroutine _visualizationInstance;
    public TabInfo[] linkedTabsByLinkID;
    public Tab[] currentlyLinkedTabs;

    public void Start()
    {
        _sourceText = GetComponent<TextMeshProUGUI>();
        _visualizationInstance = StartCoroutine(WaitThenVisualize());
    }

    private IEnumerator WaitThenVisualize()
    {
        yield return new WaitForEndOfFrame();
        SetupLinkVisuals();
        currentlyLinkedTabs = new Tab[_sourceText.textInfo.linkCount];
    }

    private IEnumerator WaitThenRemoveLinks()
    {
        yield return new WaitForEndOfFrame();
        TMP_LinkInfo[] links = _sourceText.textInfo.linkInfo; // this will nullpointer error a lot, but afaik it is impossible to check for without extending TMP
        _sourceText.text = _sourceText.text.Replace("</link>", "");
        foreach (var link in links)
        {
            _sourceText.text = _sourceText.text.Replace("<link=" + link.GetLinkID() +">", "");
        }
    }

    public void RemoveLinksForPrint()
    {
        if (_visualizationInstance != null)
        {
            StopCoroutine(_visualizationInstance);
        }
        StartCoroutine(WaitThenRemoveLinks());
    }

    private void SetupLinkVisuals()
    {
        TMP_LinkInfo[] links = _sourceText.textInfo.linkInfo;
        string _linkStartReplace = "<u color=#0000EE><color=#0000EE>";
        string _linkEndReplace = "</u></color>";
        _sourceText.text = _sourceText.text.Replace(_linkStartReplace, "");
        _sourceText.text = _sourceText.text.Replace(_linkEndReplace, "");
        for (int i = 0; i < links.Length; i++) {
            //Append behind </link>
            _sourceText.text = _sourceText.text.Insert(links[i].linkTextfirstCharacterIndex + links[i].linkTextLength + links[i].linkIdLength + 9, _linkEndReplace);
            //Place before <link
            _sourceText.text = _sourceText.text.Insert(links[i].linkTextfirstCharacterIndex, _linkStartReplace);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //Check if the left mouse button was used to click
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Finds the correct link based on the mouse position and the text field
            int linkId = TMP_TextUtilities.FindIntersectingLink(_sourceText, Input.mousePosition, null);
            if (linkId >= 0)
            {
                if (currentlyLinkedTabs[linkId] != null)
                    // check if it has a linked tab and switch to it, if it has
                {
                    BrowserManager.Instance.SetActiveTab(currentlyLinkedTabs[linkId]);
                }
                else
                {
                    // if not, make a new one based on its info and current state, and link it
                    currentlyLinkedTabs[linkId] = BrowserManager.Instance.NewTab(linkedTabsByLinkID[linkId], 0);
                }
            }
           
        }
    }
}