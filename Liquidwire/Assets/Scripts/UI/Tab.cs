using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public string tabURL;
    public bool isSecure;
    public GameObject tabBody;
    public EmailListing emailListing;
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
    public string tabHeadText;
    public string tabURL;
    public bool isSecure;
    public GameObject[] tabObjectsByState = new GameObject[3]; // defaults to 0 for ones without a case state, uses case state enum as key otherwise
}