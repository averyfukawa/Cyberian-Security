using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Editor.UI
{
    [CustomEditor(typeof(LanguageScript))]
    public class LanguageEditor : UnityEditor.Editor
    {
        private LanguageScript _languageScript;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Attach all Language scripts"))
            {
                var list = FindObjectsOfType<TextMeshProUGUI>();
                foreach (var item in list)
                {
                    GameObject temp = item.gameObject;
                    if (!temp.TryGetComponent(out TranslationScript ts))
                    {
                        temp.AddComponent<TranslationScript>();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            var enumList = LanguageScript.Languages.GetNames(typeof(LanguageScript.Languages));
            var list = FindObjectsOfType<TranslationScript>();
            
            foreach (var item in list)
            {
                var translationList = item.GetList();
                if (translationList.Count < enumList.Length)
                {
                    Debug.LogException(new Exception(item.gameObject.name + " is missing a language"));
                }

                foreach (var translation in translationList)
                {
                    if (String.IsNullOrEmpty(translation.translation))
                    {
                        Debug.LogException(new Exception(item.gameObject.name + " is missing a translation"));
                    }
                }
            }
        }
    }
}