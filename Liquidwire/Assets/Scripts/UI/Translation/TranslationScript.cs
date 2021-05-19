﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Translation;
using UnityEngine;

public class TranslationScript : MonoBehaviour
{
    public List<TranslationObject> _translation;

    private LanguageScript _languageScript;
    // Start is called before the first frame update
    void Start()
    {
        SetLanguage();
    }
    public void SetLanguage()
    {
        _languageScript = FindObjectOfType<LanguageScript>();
        foreach (var item in _translation)
        {
            if (item.language == _languageScript.languages)
            {
                gameObject.GetComponent<TextMeshProUGUI>().text = item.translation;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
