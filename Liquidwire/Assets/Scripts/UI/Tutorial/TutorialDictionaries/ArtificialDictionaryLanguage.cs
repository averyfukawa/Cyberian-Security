using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using UI.Translation;
using UnityEngine;

[Serializable]
public class ArtificialDictionaryLanguage
{
    public string name;
    [SerializeField] private LanguageScript.Languages language;
    [SerializeField] private List<ArtificialDictionaryTextParts> text = new List<ArtificialDictionaryTextParts>();

    public LanguageScript.Languages GetLanguages()
    {
        return language;
    }

    public List<ArtificialDictionaryTextParts> GetTextList()
    {
        return text;
    }
    
    public string GetTextBasedOnPart(TutorialTextPart searchTerm)
    {
        foreach (var current in text)
        {
            if (current.GetPart() == searchTerm)
            {
                return current.GetText();
            }
        }

        return null;
    }
}
