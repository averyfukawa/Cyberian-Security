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
    private GameObject camera;
    private List<TabPrefabDictionary> _tabdict;
    private List<EmailListingDictionary> _mailDict;
    public bool isInViewMode;
    public int tell;
    
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SavePlayer()
    {
        BrowserManager bm = FindObjectOfType<BrowserManager>();
        SaveSystem.SavePlayer(this, bm, FindObjectOfType<EmailInbox>().GetEmails());
    }

    public void LoadPlayer()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        SaveCube saveCube = GameObject.FindObjectOfType<SaveCube>();
        _mailDict = saveCube.mailDict;
        _tabdict = saveCube.tabdict;

        GetComponent<Movement>().isLocked = true;
        PlayerSaveData saveData = SaveSystem.LoadPlayer();

        LoadPrefabCases(saveData);
        LoadMail(saveData);
        LoadHelpFolders(saveData);
        LoadStickyNotes(saveData);

        Vector3 bodDir = new Vector3(saveData.bodyRotation[0], saveData.bodyRotation[1], saveData.bodyRotation[2]);
        transform.position = new Vector3(saveData.GetX(), saveData.GetY(), saveData.GetZ());
        transform.rotation = Quaternion.LookRotation(bodDir);
        camera.transform.LookAt(FindObjectOfType<SaveCube>().transform.position);

        // creates seperate thread.
        StartCoroutine(ReAllowMovement());
    }

    /** Find the inbox and put all the listing we have back into it using the ids we saved and a list of all the listing prefabs */
    public void LoadMail(PlayerSaveData playerSaveData)
    {
        EmailInbox inbox = FindObjectOfType<EmailInbox>();
        for (int i = 0; i < playerSaveData.mailListings.Count; i++)
        {
            if (playerSaveData.mailListings[i] <= _mailDict.Count)
            {
                inbox.LoadEmail(_mailDict[playerSaveData.mailListings[i] - 1].listing, playerSaveData.emailPosition[i],
                    playerSaveData.mailStatus[i]);
            }
        }
    }

    /** It will check the mailDict for the position that was saved in the emailListing. The emailListing list
    stores the actual caseId for a case and because the list starts at 0 we need to do -1 because the first case is 1 */
    public void LoadHelpFolders(PlayerSaveData playerSaveData)
    {
        Printer printer = FindObjectOfType<Printer>();
        int counter = 1;

        foreach (var id in playerSaveData.printedCaseIDs)
        {
            string first = id.ToString().Split(',')[0];
            int temp = (int) float.Parse(first);

            EmailListing newListing = _mailDict[playerSaveData.mailListings[temp - 1] - 1].listing
                .GetComponent<EmailListing>();
            foreach (var tabItem in _tabdict)
            {
                foreach (var mails in FindObjectOfType<EmailInbox>().GetEmails())
                {
                    if (tabItem.GetId().Equals(id))
                    {
                        if (mails.caseName == newListing.caseName)
                        {
                            FilingCabinet.Instance.CreateFolderLoad().LabelFolder(newListing.caseName,
                                "Case " + mails.caseNumber, mails.caseNumber);
                            // initiate game printing
                            Debug.Log("listId:" + newListing.tabInfo.caseNumber);
                            printer.PrintLoad(tabItem.prefab.GetComponent<Tab>(), mails.caseNumber);
                            counter++;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void LoadStickyNotes(PlayerSaveData saveData)
    {
        HelpStickyManager manager = FindObjectOfType<HelpStickyManager>();
        manager.Reset();
        manager.LoadStickyNotes(saveData.stickyIds);
    }

    /** Reset the tab list. Then we fill the tab list again with all the saved tabs so that the tabs are visible again*/
    private void LoadPrefabCases(PlayerSaveData playerSaveData)
    {
        List<float> idList = playerSaveData.tabList;
        BrowserManager bm = FindObjectOfType<BrowserManager>();
        bm.ResetList();
        for (int i = 0; i < _tabdict.Count; i++)
        {
            for (int j = 0; j < idList.Count; j++)
            {
                if (_tabdict[i].GetId().Equals(idList[j]))
                {
                    bm.SetPrefab(_tabdict[i].prefab, playerSaveData.tabInfoList[j]);
                }
            }
        }
    }

    private IEnumerator ReAllowMovement()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Movement>().isLocked = false;
    }
}