using System.Collections;
using System.Collections.Generic;
using MissionSystem;
using Player.Save_scripts.Artificial_dictionaries;
using Player.Save_scripts.Save_system_interaction;
using UI.Browser;
using UI.Browser.Emails;
using UI.Browser.Tabs;
using UnityEngine;

namespace Player.Save_scripts.Save_and_Load_scripts
{
    public class PlayerData : MonoBehaviour
    {
        /// <summary>
        /// Current instance of the PlayerData
        /// </summary>
        public static PlayerData Instance;
        private GameObject _camera;
        /// <summary>
        /// List of all the tab prefabs
        /// </summary>
        private List<TabPrefabDictionary> _tabDictionary;
        /// <summary>
        /// List of all the listing prefabs
        /// </summary>
        private List<EmailListingDictionary> _mailDictionary;
        public bool isInViewMode;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        #region Save & Load

        /// <summary>
        /// Save the all the data: "Email listings, printed pages, open tabs and all the sticky notes"
        /// </summary>
        public void SavePlayer()
        {
            BrowserManager bm = FindObjectOfType<BrowserManager>();
            SaveSystem.SavePlayer(this, bm, FindObjectOfType<EmailInbox>().GetEmails(), 
                FindObjectOfType<MissionManager>());
            Debug.Log("You saved!");
        }

        /// <summary>
        /// This function loads all of the data and references all the load methods.
        /// </summary>
        public void LoadPlayer()
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
            SaveManager saveManager = GameObject.FindObjectOfType<SaveManager>();
            _mailDictionary = saveManager.mailDictList;
            _tabDictionary = saveManager.tabDictList;

            GetComponent<Movement>().isLocked = true;
            PlayerSaveData saveData = SaveSystem.LoadPlayer();

            LoadPrefabCases(saveData);
            LoadMail(saveData);
            LoadHelpFolders(saveData);
            LoadStickyNotes(saveData);
            LoadCreatedListings(saveData);

            Vector3 bodDir = new Vector3(saveData.bodyRotation[0], saveData.bodyRotation[1], saveData.bodyRotation[2]);
            transform.position = new Vector3(saveData.GetX(), saveData.GetY(), saveData.GetZ());
            transform.rotation = Quaternion.LookRotation(bodDir);
            _camera.transform.LookAt(FindObjectOfType<SaveManager>().transform.position);

            // creates seperate thread.
            StartCoroutine(ReAllowMovement());
        }

        #endregion
        
        #region Secondary load methods

        /// <summary>
        /// Find the inbox and put all the listing we have back into it using the ids we saved and a list of all the listing prefabs 
        /// </summary>
        /// <param name="playerSaveData"></param>
        private void LoadMail(PlayerSaveData playerSaveData)
        {
            EmailInbox inbox = FindObjectOfType<EmailInbox>();
            for (int i = 0; i < playerSaveData.mailListings.Count; i++)
            {
                if (playerSaveData.mailListings[i] <= _mailDictionary.Count)
                {
                    inbox.LoadEmail(_mailDictionary[playerSaveData.mailListings[i] - 1].listing, playerSaveData.emailPosition[i],
                        playerSaveData.mailStatus[i]);
                }
            }
        }

        /// <summary>
        /// It will check the mailDictionary for the position that was saved in the emailListing. The emailListing list
        /// stores the actual caseId for a case and because the list starts at 0 we need to do -1 because the first case is 1 
        /// </summary>
        /// <param name="playerSaveData"></param>
        private void LoadHelpFolders(PlayerSaveData playerSaveData)
        {
            Printer printer = FindObjectOfType<Printer>();
            Dictionary<int, int> tempDict = new Dictionary<int, int>();
            foreach (var id in playerSaveData.GetPrinted())
            {
                string first = id.ToString().Split(',')[0];
                int temp = (int) float.Parse(first);
                if (temp != 0)
                {
                    EmailListing newListing = _mailDictionary[playerSaveData.mailListings[temp - 1] - 1].listing
                        .GetComponent<EmailListing>();
                    foreach (var tabItem in _tabDictionary)
                    {
                        foreach (var mails in FindObjectOfType<EmailInbox>().GetEmails())
                        {
                            if (tabItem.GetId().Equals(id))
                            {
                                if (mails.caseName == newListing.caseName)
                                {
                                    bool tempBool = false;
                                    foreach (var item in tempDict)
                                    {
                                        if (item.Key == temp)
                                        {
                                            foreach (var folder in FilingCabinet.Instance.caseFolders)
                                            {
                                                if (folder.caseNumber == item.Value)
                                                {
                                                    printer.PrintLoad(tabItem.prefab.GetComponent<Tab>(), mails.caseNumber);
                                                    tempBool = true;
                                                    break;
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    if (!tempBool)
                                    {
                                        FilingCabinet.Instance.CreateFolderLoad().LabelFolder(newListing.caseName,
                                            "Case " + mails.caseNumber, mails.caseNumber, mails.listingPosition);
                                        tempDict.Add(temp, mails.caseNumber);
                                        // initiate game printing
                                        printer.PrintLoad(tabItem.prefab.GetComponent<Tab>(), mails.caseNumber);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load all the saved StickyNotes.
        /// </summary>
        /// <param name="saveData"></param>
        private void LoadStickyNotes(PlayerSaveData saveData)
        {
            HelpStickyManager manager = FindObjectOfType<HelpStickyManager>();
            manager.Reset();
            manager.LoadStickyNotes(saveData.stickyIds);
        }

        /// <summary>
        /// Reset the tab list. Then we fill the tab list again with all the saved tabs so that the tabs are visible again.
        /// </summary>
        /// <param name="playerSaveData"></param>
        private void LoadPrefabCases(PlayerSaveData playerSaveData)
        {
            List<float> idList = playerSaveData.tabList;
            BrowserManager bm = FindObjectOfType<BrowserManager>();
            bm.ResetList();
            for (int i = 0; i < _tabDictionary.Count; i++)
            {
                for (int j = 0; j < idList.Count; j++)
                {
                    if (_tabDictionary[i].GetId().Equals(idList[j]))
                    {
                        bm.SetPrefab(_tabDictionary[i].prefab, playerSaveData.tabInfoList[j]);
                    }
                }
            }
        }

        private void LoadCreatedListings(PlayerSaveData playerSaveData)
        {
            MissionManager manager = FindObjectOfType<MissionManager>();
            manager.LoadManagerState(playerSaveData.GetCreatedList());
        }
        #endregion
        
        /// <summary>
        /// Wait a second and then turn on the movement again.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ReAllowMovement()
        {
            yield return new WaitForSeconds(1);
            GetComponent<Movement>().isLocked = false;
        }
    }
}