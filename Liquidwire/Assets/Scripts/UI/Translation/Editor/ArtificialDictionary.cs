using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ArtificialDictionary
{
    private bool allowWarning = true;
    private string objectName;

    public ArtificialDictionary(string objectName)
    {
        this.objectName = objectName;
    }

    public bool GetWarning()
    {
        return allowWarning;
    }

    public string GetName()
    {
        return objectName;
    }
}
