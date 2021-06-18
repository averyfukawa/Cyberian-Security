using System;
using System.Collections;
using System.Collections.Generic;
using Player.Save_scripts.Artificial_dictionaries;
using Player.Save_scripts.Save_system_interaction;
using TMPro;
using UI.Browser.Tabs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Browser.Emails
{
    public class EmailListing : MonoBehaviour
    {
        /// <summary>
        /// Stores the information of the tab
        /// </summary>
        public TabInfo tabInfo;

        /// <summary>
        /// The difficulty of the case
        /// </summary>
        [Range(1, 5)] public int difficultyValue;

        public CaseStatus currentStatus = CaseStatus.Unopened;
        public int caseNumber; // a 1 indexed int

        /// <summary>
        /// Position of the prefab in the list
        /// </summary>
        public int listingPosition = 0;

        public string caseName;
        public Tab linkedTab;

        public bool isStoryMission;
        public bool isStoryLineStart;
        public int prerequisiteMissionId = 0;
        private SaveManager _manager;

        [SerializeField] private TextMeshProUGUI _nameField;
        [SerializeField] private TextMeshProUGUI _statusField;
        [SerializeField] private Image[] _difficultyIndicators;
        [SerializeField] private Color[] _difficultyColours = new Color[4];

        public enum CaseStatus
        {
            Unopened,
            Started,
            Conclusion
        }

        private void Start()
        {
            _manager = FindObjectOfType<SaveManager>();
        }

        /// <summary>
        /// Set the visuals for the email listings.
        /// </summary>
        public void SetVisuals()
        {
            Color diffColour = _difficultyColours[Mathf.FloorToInt((float) (difficultyValue - 1) / 2)];
            for (int i = 0; i < 5; i++)
            {
                if (i + 1 <= difficultyValue)
                {
                    _difficultyIndicators[i].color = diffColour;
                }
                else
                {
                    _difficultyIndicators[i].color = _difficultyColours[_difficultyColours.Length - 1];
                }
            }

            _nameField.text = "Case " + caseNumber + " " + caseName;
            _statusField.text = currentStatus.ToString();
            if (tabInfo.tabHeadText == "")
            {
                tabInfo.tabHeadText = "Case - " + caseNumber;
            }

            if (tabInfo.tabURL == "emailCase")
            {
                tabInfo.caseNumber = caseNumber;
                tabInfo.tabURL = BrowserManager.Instance.tabList[0].tabURL + "/case" + caseNumber;
            }
        }

        /// <summary>
        /// Opens the tab and created the folder for the case
        /// </summary>
        public void OpenEmail()
        {
            if (linkedTab != null)
                // check if it has a linked tab and switch to it, if it has
            {
                BrowserManager.Instance.SetActiveTab(linkedTab);
            }
            else
            {
                // if not, make a new one based on its info and current state, and link it
                switch (currentStatus)
                {
                    case CaseStatus.Unopened:
                        // change the status if it was unopened, and when you do, refresh
                        currentStatus++;
                        SetVisuals();
                        FilingCabinet.Instance.CreateFolder().LabelFolder(_nameField.text, "Case " + caseNumber, caseNumber,
                            listingPosition);
                        linkedTab = BrowserManager.Instance.NewTab(tabInfo, 0);
                        break;
                    case CaseStatus.Conclusion:
                        // TODO add reply email from happy clients here to reinforce learning effect for player
                        break;
                    case CaseStatus.Started:
                        OpenTabsRelated();
                        linkedTab = BrowserManager.Instance.NewTab(tabInfo, 0);
                        // TODO replace this with a run through of non printed pages belonging to this case and open them instead
                        break;
                }
            }
        }

        private void OpenTabsRelated()
        {
            foreach (var currentMailDict in _manager.tabDictList)
            {
                var idTemp = GetCaseID(currentMailDict.prefab);
                if (idTemp == caseNumber)
                {
                    Debug.Log("wack: " + currentMailDict.prefab.name);
                    BrowserManager.Instance.NewTab(currentMailDict.prefab.gameObject.GetComponent<Tab>().tabInfo, 0);
                }
            }
        }
        
        private int GetCaseID(GameObject current)
        {
            String temp = current.name.Split(' ')[1];
            string[] tempArr = temp.Split('.');
            Debug.Log("int: " + Int32.Parse(tempArr[0]));
            return Int32.Parse(tempArr[0]);
        }

        public void SetStoryLine(int preMissionID)
        {
            this.isStoryMission = true;
            this.prerequisiteMissionId = preMissionID;

        }

        public void RemoveStoryLink()
        {

            this.isStoryMission = false;

            // set to 0 because no listing has that number
            this.prerequisiteMissionId = 0;
        }

        public void LogConnection()
        {
            List<EmailListingDictionary> _missionCases = FindObjectOfType<SaveManager>().GetComponent<SaveManager>().mailDictList;

            EmailListing preqmiss = null;


            foreach (var mission in _missionCases)
            {
                if (mission.listing.GetComponent<EmailListing>().listingPosition == prerequisiteMissionId)
                {
                    preqmiss = mission.listing.GetComponent<EmailListing>();
                }

            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(EmailListing))]
    public class EmailListingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Refresh Values"))
            {
                EmailListing listing = target as EmailListing;
                listing.SetVisuals();
            }
        }
    }
    #endif
}