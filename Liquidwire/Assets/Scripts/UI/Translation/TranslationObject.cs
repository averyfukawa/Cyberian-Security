using System;
using System.Collections;
using System.Collections.Generic;
using UI.Translation;
using UnityEngine;
[Serializable]
public class TranslationObject
{
    public LanguageScript.Languages language;
    [TextArea(3,10)]public string translation;

    public TranslationObject(string translation, LanguageScript.Languages language)
    {
        this.language = language;
        this.translation = translation;
    }
}
