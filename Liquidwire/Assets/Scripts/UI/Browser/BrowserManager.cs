using System.Collections.Generic;
using Player.Save_scripts.Save_system_interaction;
using TMPro;
using UI.Browser.Tabs;
using UnityEngine;

namespace UI.Browser
{
    public class BrowserManager : MonoBehaviour
    {
        /// <summary>
        /// Instance of the current script
        /// </summary>
        public static BrowserManager Instance;
        [SerializeField] private Transform _untabOverlay;
        [SerializeField] private Transform _adressBarTrans;
        [SerializeField] private TextMeshProUGUI _adressBar;
        [SerializeField] private GameObject _tabSecureIcon;
        [SerializeField] private GameObject _printButton;
        private Dictionary<float, bool> _pagePrintStatus = new Dictionary<float, bool>();
        /// <summary>
        /// List with all the tabs that are open
        /// </summary>
        public List<Tab> tabList = new List<Tab>();
        /// <summary>
        /// Current active tab
        /// </summary>
        public Tab activeTab;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            _printButton.SetActive(false);

            SaveManager saveMan = FindObjectOfType<SaveManager>();
            foreach (var entry in saveMan.tabDictList)
            {
                _pagePrintStatus.Add(entry.GetId(), false);
            }
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
        }

        public void PrintCurrentPage()
        {
            if (_pagePrintStatus[activeTab.tabId])
            {
                return;
            }
            else
            {
                _pagePrintStatus[activeTab.tabId] = true;
            }
            Printer.Instance.Print(activeTab, activeTab.caseNumber, false);

            if (TutorialManager.Instance._doTutorial &&
                TutorialManager.Instance.currentState == TutorialManager.TutorialState.EmailThree)
            {
                TutorialManager.Instance.AdvanceTutorial();
            }
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
                if (TutorialManager.Instance._doTutorial && TutorialManager.Instance.currentState == TutorialManager.TutorialState.EmailTwo)
                {
                    TutorialManager.Instance.AdvanceTutorial();
                }
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

        public Dictionary<float, bool> GetPrintStatus()
        {
            return _pagePrintStatus;
        }

        public void SetPrintStatus(List<PrintStatusSave> loadSave)
        {
            foreach (var currentLoad in loadSave)
            {
                _pagePrintStatus[currentLoad.id] = currentLoad.printed;
            }
        }

        public bool CheckPrintStatus(int caseID)
        {
            foreach (var key in _pagePrintStatus.Keys)
            {
                if (Mathf.FloorToInt(key) == caseID)
                {
                    if (!_pagePrintStatus[key])
                    {
                        return false; // at least one was not printed
                    }
                }
            }

            return true; // all were printed, or the system failed =)
        }
    }
}
