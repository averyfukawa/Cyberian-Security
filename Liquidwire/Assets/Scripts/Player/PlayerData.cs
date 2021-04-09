using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public bool isAtComputer;
    private GameObject camera;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        bool temp = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        GetComponent<Movement>().isLocked = true;
        PlayerSaveData saveData = SaveSystem.LoadPlayer();

        transform.position = new Vector3(saveData.GetX(), saveData.GetY(), saveData.GetZ());
        
        StartCoroutine(Wait());

        Vector3 bodDir = new Vector3(saveData.bodyRotation[0],saveData.bodyRotation[1],saveData.bodyRotation[2]);
        
        transform.rotation = Quaternion.LookRotation(bodDir);
        camera.transform.LookAt(FindObjectOfType<SaveCube>().transform.position);
        

        // creates seperate thread.
        StartCoroutine(ReAllowMovement());
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