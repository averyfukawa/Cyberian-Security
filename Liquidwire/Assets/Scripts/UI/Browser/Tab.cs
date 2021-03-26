using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public string tabURL;
    public bool isSecure;
    public bool isPrintable;
    public GameObject tabBody;
    public GameObject _printableChildObject;
    public int caseNumber;
    [SerializeField] private TextMeshProUGUI _tabHeadText;
    [SerializeField] private RectTransform _tabHeadTrans;
    [SerializeField] private Vector3 _tabHeadBaseOffset;

    public void IndentHead(int tabIndex, bool firstSet)
    {
        if (firstSet)
        {
            _tabHeadTrans.LeanMove(_tabHeadBaseOffset + new Vector3(tabIndex * _tabHeadTrans.rect.width, 0, 0), 0.001f);
        }
        else
        {
            _tabHeadTrans.LeanMove(_tabHeadBaseOffset + new Vector3(tabIndex * _tabHeadTrans.rect.width, 0, 0), 0.5f);
        }
    }
    
    public void SetInfo(TabInfo info)
    {
        _tabHeadText.text = info.tabHeadText;
        tabURL = info.tabURL;
        isSecure = info.isSecure;
        if (info.caseNumber != 0)
        {
            caseNumber = info.caseNumber;
        }
    }

    public void SetActive()
    {
        BrowserManager.Instance.SetActiveTab(this);
    }

    public void Close()
    {
        BrowserManager.Instance.CloseTab(this);
    }
}

[Serializable]
public class TabInfo
{
    // used to accurately fill the fields of the browser
    public string tabHeadText; // defaults set for emails in the EmailListing
    public string tabURL; // defaults set for emails in the EmailListing
    public bool isSecure;
    public GameObject[] tabObjectsByState = new GameObject[3]; // defaults to 0 for ones without a case state, uses case state enum as key otherwise
    public int caseNumber;
}