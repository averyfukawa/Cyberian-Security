using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Browser.Tabs;
using UnityEngine;

public class BrowserManager : MonoBehaviour
{
    public static BrowserManager Instance;
    [SerializeField] private Transform _untabOverlay;
    [SerializeField] private Transform _adressBarTrans;
    [SerializeField] private TextMeshProUGUI _adressBar;
    [SerializeField] private GameObject _tabSecureIcon;
    [SerializeField] private GameObject _printButton;
    public List<Tab> tabList = new List<Tab>();
    public Tab activeTab;

    private void Start()
    {
        if (Instance == null)
        {
            
            Instance = this;
        }
        _printButton.SetActive(false);
    }
    public void PrintCurrentPage()
    {
        Printer.Instance.Print(activeTab, activeTab.caseNumber);
    }

    public void ResetList()
    {

        foreach (var tab in tabList)
        {
            Destroy(tab.gameObject);
            
        }
           
        tabList = new List<Tab>();
    }

    #region Tab methods
    /// <summary>
    /// Set the prefab of the tab.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="saveInfo"></param>
    public void SetPrefab(GameObject go, SaveInfo saveInfo)
    {
        Tab newTab = Instantiate(go, transform).GetComponent<Tab>();
        newTab.SetInfo(new TabInfo(saveInfo.tabHeadText, saveInfo.tabURL, saveInfo.isSecure, saveInfo.caseNumber));
        newTab.IndentHead(tabList.Count, true);
        tabList.Add(newTab);
        SetActiveTab(tabList[0]);
    }
    
    public void CloseTab(Tab tabToClose)
    {
        bool afterClosed = false;
        for (int i = 0; i < tabList.Count; i++)
        {
            if (tabList[i].Equals(tabToClose))
            {
                afterClosed = true;
                if (tabToClose.Equals(activeTab))
                {
                    SetActiveTab(tabList[i-1]);
                }
            }
            else if(afterClosed)
            {
                tabList[i].IndentHead(i-1, false);
            }
        }
        tabList.Remove(tabToClose);
        Destroy(tabToClose.gameObject);
        // TODO add additional functionality for half finished cases here
    }
    
    public Tab NewTab(TabInfo newTabInfo, int tabKey)
    {
        if (tabList.Count < 4)
        {
            Tab newTab = Instantiate(newTabInfo.tabObjectsByState[tabKey], transform).GetComponent<Tab>();
            newTab.IndentHead(tabList.Count, true);
            tabList.Add(newTab);
            newTab.SetInfo(newTabInfo);
            SetActiveTab(newTab);
            return newTab;
        }
        else
        {
            Debug.Log("Solve some cases first, you swine !");
            return null;
        }
    }

    public void SetActiveTab(Tab newActiveTab)
    {
        if (activeTab != null)
        {
            //activeTab.tabBody.SetActive(false);
        }
        newActiveTab.tabBody.SetActive(true);
        _untabOverlay.SetAsLastSibling();
        newActiveTab.transform.SetAsLastSibling();
        _adressBarTrans.SetAsLastSibling();
        _adressBar.text = newActiveTab.tabURL;
        _tabSecureIcon.SetActive(newActiveTab.isSecure);
        activeTab = newActiveTab;
        _printButton.SetActive(newActiveTab.isPrintable);
    }

    #endregion
}
