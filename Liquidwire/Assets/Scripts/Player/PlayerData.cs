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
        SaveSystem.SavePlayer(this, bm);
        
        //loop door list heen en pak key value
        
    }

    public void LoadPlayer()
    {
        bool temp = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        GetComponent<Movement>().isLocked = true;
        PlayerSaveData saveData = SaveSystem.LoadPlayer();

        LoadPrefabCases(saveData);
        transform.position = new Vector3(saveData.GetX(), saveData.GetY(), saveData.GetZ());
        
        StartCoroutine(Wait());

        Vector3 bodDir = new Vector3(saveData.bodyRotation[0],saveData.bodyRotation[1],saveData.bodyRotation[2]);
        
        transform.rotation = Quaternion.LookRotation(bodDir);
        camera.transform.LookAt(FindObjectOfType<SaveCube>().transform.position);
        
        

        // creates seperate thread.
        StartCoroutine(ReAllowMovement());
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