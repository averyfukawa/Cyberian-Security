using System;
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
        FolderMenu.setLanguageEvent += SetLanguage;
    }

    private void SetLanguage()
    {
        _languageScript = FindObjectOfType<LanguageScript>();
        foreach (var item in _translation)
        {
            if (item.language == _languageScript.currentLanguage)
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
