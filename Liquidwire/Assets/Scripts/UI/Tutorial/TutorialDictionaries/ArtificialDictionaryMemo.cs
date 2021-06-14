using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using UI.Translation;
using UnityEngine;

[Serializable]
public class ArtificialDictionaryMemo
{
    public string name;
    [SerializeField] private LanguageScript.Languages language;
    [SerializeField] private List<ArtificialDictionaryMemoParts> text = new List<ArtificialDictionaryMemoParts>();

    public LanguageScript.Languages GetLanguages()
    {
        return language;
    }

    public List<ArtificialDictionaryMemoParts> GetTextList()
    {
        return text;
    }
    
    public string GetTextBasedOnState(TutorialManager.TutorialState searchState)
    {
        foreach (var current in text)
        {
            if (current.GetState() == searchState)
            {
                return current.GetText();
            }
        }

        return null;
    }
}
