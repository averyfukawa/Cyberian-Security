using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Browser.Tabs
{
    [Serializable]
    public class Tab : MonoBehaviour
    {
        public float tabId;
        public string tabURL;
        public bool isSecure;
        public bool isPrintable;
        public GameObject tabBody;
        public GameObject _printableChildObject;
        public int caseNumber;
        public TabInfo tabInfo;
        [SerializeField] private TextMeshProUGUI _tabHeadText;
        [SerializeField] private RectTransform _tabHeadTrans;
        [SerializeField] private Vector3 _tabHeadBaseOffset;

        public void Start()
        {
            if (tabId == 0)
            {
                SetTabID();
            }
        }

        public void SetTabID()
        {
            String temp = gameObject.name.Split(' ')[1];
            string[] tempArr = temp.Split('.');
            string key;
            if (tempArr.Length > 1)
            {
                key = tempArr[0] + "." + tempArr[1];
            }
            else
            {
                key = "0";
            }

            tabId = (float.Parse(key, new System.Globalization.CultureInfo("en-US", false)));
        }

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
        
        public void Close()
        {
            BrowserManager.Instance.CloseTab(this);
        }
        
        #region Setters

        public void SetInfo(TabInfo info)
        {
            _tabHeadText.text = info.tabHeadText;
            tabURL = info.tabURL;
            isSecure = info.isSecure;
            if (info.caseNumber != 0)
            {
                caseNumber = info.caseNumber;
            }

            tabInfo = info;
        }

        public void SetActive()
        {
            BrowserManager.Instance.SetActiveTab(this);
        }

        #endregion
    }

    #region Info classes

    [Serializable]
    public class TabInfo
    {
        // used to accurately fill the fields of the browser
        public string tabHeadText; // defaults set for emails in the EmailListing
        public string tabURL; // defaults set for emails in the EmailListing
        public bool isSecure;

        public GameObject[]
            tabObjectsByState =
                new GameObject[3]; // defaults to 0 for ones without a case state, uses case state enum as key otherwise

        public int caseNumber;

        public TabInfo(string tabHeadText, string tabURL, bool isSecure, int caseNumber)
        {
            this.tabHeadText = tabHeadText;
            this.tabURL = tabURL;
            this.isSecure = isSecure;
            this.caseNumber = caseNumber;
        }
    }

    [Serializable]
    public class SaveInfo
    {
        // used to accurately fill the fields of the browser
        public string tabHeadText; // defaults set for emails in the EmailListing
        public string tabURL; // defaults set for emails in the EmailListing
        public bool isSecure;
        public int caseNumber;

        public SaveInfo(string tabHeadText, string tabURL, bool isSecure, int caseNumber)
        {
            this.tabHeadText = tabHeadText;
            this.tabURL = tabURL;
            this.isSecure = isSecure;
            this.caseNumber = caseNumber;
        }
    }

    #endregion
}