using System;
using System.Collections;
using System.Collections.Generic;
using UI.Translation;
using UnityEngine;
[Serializable]
public class TranslationObject
{
    public LanguageScript.Languages language;
    public string translation;

    public TranslationObject(string translation, LanguageScript.Languages language)
    {
        this.language = language;
        this.translation = translation;
    }
}
