using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenuButtons : MonoBehaviour
{
    private PlayerData pd;
    private Movement move;
    private MouseCamera mc;
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
        mc.SetCursorLocked();
        move.isLocked = false;
        pd.LoadPlayer();
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        mc.SetCursorLocked();
        move.isLocked = false;
    }
}
