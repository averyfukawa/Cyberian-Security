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
    public bool isInViewMode;
    public int tell;
    
    //todo find a better place for this var
    public List<TabPrefabDictionary> tabdict;
    public List<EmailListingDictionary> mailDict;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        foreach (var item in tabdict)
        {
            item.SetId();
        }
    }

    public void SavePlayer()
    {
        BrowserManager bm = FindObjectOfType<BrowserManager>();
        SaveSystem.SavePlayer(this, bm, FindObjectOfType<EmailInbox>().GetEmails());
    }

    public void LoadPlayer()
    {
        bool temp = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        
        
        GetComponent<Movement>().isLocked = true;
        
        // retrieve all the  save data
        PlayerSaveData saveData = SaveSystem.LoadPlayer();
        
        // set the game
        
        //todo functionize this. 
        LoadPrefabCases(saveData);
        EmailInbox inbox = FindObjectOfType<EmailInbox>();
        for (int i =0; i < saveData.mailListings.Count; i++)
        {
            if (saveData.mailListings[i] <= mailDict.Count)
            {
                inbox.LoadEmail(mailDict[saveData.mailListings[i]-1].listing, saveData.emailPosition[i],  saveData.mailStatus[i]);
            }
        }
        
        transform.position = new Vector3(saveData.GetX(), saveData.GetY(), saveData.GetZ());
        
        StartCoroutine(Wait());
        
        LoadStickyNotes(saveData);

        Vector3 bodDir = new Vector3(saveData.bodyRotation[0],saveData.bodyRotation[1],saveData.bodyRotation[2]);
        
        transform.rotation = Quaternion.LookRotation(bodDir);
        camera.transform.LookAt(FindObjectOfType<SaveCube>().transform.position);
        
        

        // creates seperate thread.
        StartCoroutine(ReAllowMovement());
    }

    public void LoadStickyNotes(PlayerSaveData saveData)
    {
        HelpStickyManager manager = FindObjectOfType<HelpStickyManager>();
        manager.Reset();
        manager.LoadStickyNotes(saveData.stickyIds);
    }

    private void LoadPrefabCases(PlayerSaveData playerSaveData)
    {
        List<float> idList = playerSaveData.tabList;
        BrowserManager bm = FindObjectOfType<BrowserManager>();
        bm.ResetList();
        for (int i = 0; i < tabdict.Count; i++)
        {
            for(int j = 0; j < idList.Count; j++)
            {
                if (tabdict[i].GetId().Equals(idList[j]))
                {
                    bm.SetPrefab(tabdict[i].prefab, playerSaveData.tabInfoList[j]);
                }
            }
        }
    }

    private IEnumerator ReAllowMovement()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Movement>().isLocked = false;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
    
    
}