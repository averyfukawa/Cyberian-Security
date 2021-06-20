using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Browser;
using UI.Browser.Tabs;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

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
        Tab thisTab = GetComponentInParent<Tab>();
        foreach (var tab in linkedTabsByLinkID)
        {
            if (tab.tabHeadText == "")
            {
                tab.tabHeadText = "Case - " + thisTab.caseNumber;
            }

            if (tab.tabURL == "attachment")
            {
                tab.tabURL = thisTab.tabURL + "/attachment/" + RandomUrl(6);
            }
        }
    }

    #region Wait methods
    /// <summary>
    /// Wait until everything has been done this frame and then set the visuals for the links
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitThenVisualize()
    {
        yield return new WaitForEndOfFrame();
        SetupLinkVisuals();
        currentlyLinkedTabs = new Tab[_sourceText.textInfo.linkCount];
    }
    /// <summary>
    /// remove the link at the end of the frame
    /// </summary>
    /// <returns></returns>
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

    #endregion
    
    #region Link methods
    /// <summary>
    /// Remove the links before printing.
    /// </summary>
    public void RemoveLinksForPrint()
    {
        if (_visualizationInstance != null)
        {
            StopCoroutine(_visualizationInstance);
        }
        // StartCoroutine(WaitThenRemoveLinks()); removed due to other systems taking this job #automationKillsWorkers !
    }
    /// <summary>
    /// Set the visuals for the links.
    /// </summary>
    private void SetupLinkVisuals()
    {
        TMP_LinkInfo[] links = _sourceText.textInfo.linkInfo;
        string _linkStartReplace = "<u color=#0000EE><color=#0000EE>";
        string _linkEndReplace = "</u></color>";
        _sourceText.text = _sourceText.text.Replace(_linkStartReplace, "");
        _sourceText.text = _sourceText.text.Replace(_linkEndReplace, "");
        for (int i = 0; i < links.Length; i++) {
            //Append behind </link>
            _sourceText.text = _sourceText.text.Insert(links[i].linkTextfirstCharacterIndex + links[i].linkTextLength + links[i].linkIdLength + 7, _linkEndReplace);
            //Place before <link
            _sourceText.text = _sourceText.text.Insert(links[i].linkTextfirstCharacterIndex, _linkStartReplace);
        }
    }

    #endregion
    
    private string RandomUrl(int length)
    {
        string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";  
        Random random = new Random();  
        char[] chars = new char[length];  
        for (int i = 0; i < length; i++)  
        {  
            chars[i] = validChars[random.Next(0, validChars.Length)];  
        }  
        return new string(chars); 
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
                GameObject mouseClick = GameObject.FindGameObjectWithTag("SFX");
                mouseClick.GetComponent<SFX>().SoundMouseClick();
                
                if (currentlyLinkedTabs[linkId] != null)
                    // check if it has a linked tab and switch to it, if it has
                {
                    BrowserManager.Instance.SetActiveTab(currentlyLinkedTabs[linkId]);
                }
                else
                {
                    // if not, make a new one based on its info and current state, and link it
                    Tab newTab = linkedTabsByLinkID[linkId].tabObjectsByState[0].GetComponent<Tab>();
                    newTab.SetTabID();
                    bool exists = false;
                    foreach (var currentTabInstance in BrowserManager.Instance.tabList)
                    {
                        if (currentTabInstance.tabId == newTab.tabId)
                        {
                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        linkedTabsByLinkID[linkId].caseNumber = BrowserManager.Instance.activeTab.caseNumber;
                        currentlyLinkedTabs[linkId] = BrowserManager.Instance.NewTab(linkedTabsByLinkID[linkId]);
                    }
                    
                }
            }
           
        }
    }
}