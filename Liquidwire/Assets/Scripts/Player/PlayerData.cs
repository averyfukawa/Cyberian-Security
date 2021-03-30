using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public bool isAtComputer;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
