using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class MenuButtons : MonoBehaviour
{
    private PlayerData pd;
    private Movement move;
    private MouseCamera mc;
    [SerializeField] private GameObject menuCam;

    public void Start()
    {
        pd = FindObjectOfType<PlayerData>();
        move = pd.gameObject.GetComponent<Movement>();
        move.isLocked = true;
        mc = move.gameObject.GetComponentInChildren<MouseCamera>();
        mc.SetCursorNone();
    }

    public void Update()
    {
        if (mc.GetLockedState())
        {
            mc.SetCursorNone();
        }
    }

    public void LoadPlayer()
    {
        pd.LoadPlayer();
        StartCoroutine(DoCamTransition());
    }

    public void StartGame()
    {
        StartCoroutine(DoCamTransition());
    }

    public void MenuAudio()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        
    }

    private IEnumerator DoCamTransition()
    {
        mc.SetCursorLocked();
        foreach (var btn in transform.GetComponentsInChildren<Button>())
        {
            btn.gameObject.SetActive(false);
        }
        Transform mainCamTrans = Camera.main.transform;
        menuCam.LeanMove(mainCamTrans.position, 1.5f);
        menuCam.LeanRotate(mainCamTrans.rotation.eulerAngles, 1.5f);
        yield return new WaitForSeconds(1.5f);
        move.isLocked = false;
        menuCam.SetActive(false);
    }
}
