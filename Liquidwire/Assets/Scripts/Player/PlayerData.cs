using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public bool isInViewMode;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
