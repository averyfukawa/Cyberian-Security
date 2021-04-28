using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UI.Browser;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public bool isAtComputer;
    private GameObject _camera;
    private List<TabPrefabDictionary> _tabDictionary;
    private List<EmailListingDictionary> _mailDictionary;
    public bool isInViewMode;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Save the all the data: "Email listings, printed pages, open tabs and all the sticky notes"
    /// </summary>
    public void SavePlayer()
    {
        BrowserManager bm = FindObjectOfType<BrowserManager>();
        SaveSystem.SavePlayer(this, bm, FindObjectOfType<EmailInbox>().GetEmails());
        Debug.Log("You saved!");
    }

    /// <summary>
    /// This function loads all of the data and references all the load methods.
    /// </summary>
    public void LoadPlayer()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        SaveCube saveCube = GameObject.FindObjectOfType<SaveCube>();
        _mailDictionary = saveCube.mailDict;
        _tabDictionary = saveCube.tabdict;

        GetComponent<Movement>().isLocked = true;
        PlayerSaveData saveData = SaveSystem.LoadPlayer();

        LoadPrefabCases(saveData);
        LoadMail(saveData);
        LoadHelpFolders(saveData);
        LoadStickyNotes(saveData);

        Vector3 bodDir = new Vector3(saveData.bodyRotation[0], saveData.bodyRotation[1], saveData.bodyRotation[2]);
        transform.position = new Vector3(saveData.GetX(), saveData.GetY(), saveData.GetZ());
        transform.rotation = Quaternion.LookRotation(bodDir);
        _camera.transform.LookAt(FindObjectOfType<SaveCube>().transform.position);

        // creates seperate thread.
        StartCoroutine(ReAllowMovement());
    }

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
        int counter = 1;
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
                                    counter++;
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