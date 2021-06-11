using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PrintStatusSave
{
    [SerializeField]public bool printed;
    [SerializeField]public float id;

    public PrintStatusSave(float id, bool printed)
    {
        this.id = id;
        this.printed = printed;
    }   
}
