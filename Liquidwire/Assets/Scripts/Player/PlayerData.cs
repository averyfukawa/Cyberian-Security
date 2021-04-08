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

    private void Update()
    {
        // Debug.Log( "transform "+ "y: "+ transform.position.y + "x: " +  transform.position.x  + "z: " +  transform.position.z);
    }

    public void SavePlayer()
    {
        // PlayerSaveData playerSaveData = new PlayerSaveData(this);

        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        bool temp = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        GetComponent<Movement>().isLocked = true;
        PlayerSaveData saveData = SaveSystem.LoadPlayer();

        // this.transform.position.Set(saveData.GetX(),saveData.GetY(),saveData.GetZ());

        transform.position = new Vector3(saveData.GetX(), saveData.GetY(), saveData.GetZ());
        
        
        Debug.Log("eu: " + camera.transform.eulerAngles);
        StartCoroutine(Wait());
        // transform.rotation = Quaternion.Euler(new Vector3(saveData.bodyRotation[0],saveData.bodyRotation[1],saveData.bodyRotation[2]));
        // transform.rotation = new Quaternion(0, saveData.bodyRotation[1], 0, 0);
        Vector3 bodDir = new Vector3(saveData.bodyRotation[0],saveData.bodyRotation[1],saveData.bodyRotation[2]);
        Vector3 camDir = new Vector3(saveData.cameraRotation[0],saveData.cameraRotation[1],saveData.cameraRotation[2]);
        
        Debug.Log("camDir: "+ camDir);
        Debug.Log("bodDir: "+ bodDir);

        transform.rotation = Quaternion.LookRotation(bodDir);
        camera.transform.LookAt(FindObjectOfType<SaveCube>().transform.position);

        //todo main camera veranderen

        
        //todo personage verzetten.
        //todo 

        // t.transform.position = new Vector3(1f,2f,3f);

        // transform.position = new Vector3(1f, 2f, 3f);

        Debug.Log("new position is" + transform.position.x + " " + transform.position.y + " " + transform.position.x);


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