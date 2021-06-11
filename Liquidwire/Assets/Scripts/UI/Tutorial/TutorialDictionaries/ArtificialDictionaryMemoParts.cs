using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;

[Serializable]
public class ArtificialDictionaryMemoParts
{
    public string name;
    [SerializeField, TextArea(0, 10)] private string text;
    [SerializeField] private TutorialManager.TutorialState _relatedState;

    public string GetText()
    {
        return text;
    }

    public TutorialManager.TutorialState GetState()
    {
        return _relatedState;
    }
}