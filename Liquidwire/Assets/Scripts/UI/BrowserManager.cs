using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BrowserManager : MonoBehaviour
{
    public static BrowserManager Instance;
    [SerializeField] private Transform _untabOverlay;
    [SerializeField] private Transform _adressBarTrans;
    [SerializeField] private TextMeshProUGUI _adressBar;
    [SerializeField] private GameObject _tabSecureIcon;
    public List<Tab> tabList = new List<Tab>();
    private Tab _activeTab;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void CloseTab(Tab tabToClose)
    {
        bool afterClosed = false;
        for (int i = 0; i < tabList.Count; i++)
        {
            if (tabList[i].Equals(tabToClose))
            {
                afterClosed = true;
                if (tabToClose.Equals(_activeTab))
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
        if (_activeTab != null)
        {
            _activeTab.tabBody.SetActive(false);
        }
        newActiveTab.tabBody.SetActive(true);
        _untabOverlay.SetAsLastSibling();
        newActiveTab.transform.SetAsLastSibling();
        _adressBarTrans.SetAsLastSibling();
        _adressBar.text = newActiveTab.tabURL;
        _tabSecureIcon.SetActive(newActiveTab.isSecure);
        _activeTab = newActiveTab;
    }
}
