using System;
using System.Collections;
using System.Collections.Generic;
using UI.Translation;
using UnityEngine;
[Serializable]
public class CTATranslationDict
{
    public LanguageScript.Languages language;
    [TextArea(3,10)]public string translationHover;
    [TextArea(3,10)]public string translationInspect;
    

    public CTATranslationDict(string translationHover, string translationInspect, LanguageScript.Languages language)
    {
        this.language = language;
        this.translationHover = translationHover;
        this.translationInspect = translationInspect;
    }
}
