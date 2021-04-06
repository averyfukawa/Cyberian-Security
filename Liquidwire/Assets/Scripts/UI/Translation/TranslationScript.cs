using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslationScript : MonoBehaviour
{
    [SerializeField] private List<TranslationObject> _translation;

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

    public List<TranslationObject> GetList()
    {
        return _translation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
