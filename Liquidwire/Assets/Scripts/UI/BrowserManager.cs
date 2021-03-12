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
    [SerializeField] private GameObject _tabPrefab;
    [SerializeField] private GameObject _tabSecureIcon;
    private List<Tab> _tabList = new List<Tab>();
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
        for (int i = 0; i < _tabList.Count; i++)
        {
            if (_tabList[i].Equals(tabToClose))
            {
                afterClosed = true;
                if (tabToClose.Equals(_activeTab))
                {
                    SetActiveTab(_tabList[i-1]);
                }
            }
            else if(afterClosed)
            {
                _tabList[i].IndentHead(i-1, false);
            }
        }
        _tabList.Remove(tabToClose);
        Destroy(tabToClose.gameObject);
        // TODO add additional functionality for half finished cases here
    }

    public void NewTestTab()
    {
        NewTab(new TabInfo("Testing Tab", "HTTPS://www.testing.tab.com/testTab", true));
    }

    public void NewTab(TabInfo newTabInfo)
    {
        if (_tabList.Count < 4)
        {
            Tab newTab = Instantiate(_tabPrefab, transform).GetComponent<Tab>();
            newTab.IndentHead(_tabList.Count, true);
            _tabList.Add(newTab);
            newTab.SetInfo(newTabInfo);
            SetActiveTab(newTab);
        }
        else
        {
            Debug.Log("Solve some cases first, you swine !");
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
