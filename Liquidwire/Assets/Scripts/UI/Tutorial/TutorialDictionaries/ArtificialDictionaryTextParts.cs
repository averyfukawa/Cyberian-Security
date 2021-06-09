using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;

[Serializable]
public class ArtificialDictionaryTextParts
{
    public string name;
    [SerializeField, TextArea(0, 10)] private string text;
    [SerializeField] private TutorialTextPart textPart;

    public string GetText()
    {
        return text;
    }

    public TutorialTextPart GetPart()
    {
        return textPart;
    }
}