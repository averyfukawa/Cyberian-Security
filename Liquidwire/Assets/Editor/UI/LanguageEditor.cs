﻿using System;
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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Attach all Language scripts"))
            {
                var list = FindObjectsOfType<TextMeshProUGUI>();
                Undo.RecordObjects(list, "Assign Translation Objects");
                foreach (var item in list)
                {
                    GameObject temp = item.gameObject;
                    LanguageScript LS = target as LanguageScript;
                    if (!temp.TryGetComponent(out TranslationScript ts))
                    {
                        TranslationScript tempClass = temp.AddComponent<TranslationScript>();
                        tempClass._translation = new List<TranslationObject>(LS.LanguageNumber());
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
                if (item._translation.Count < enumList.Length)
                {
                    Debug.LogException(new Exception(item.gameObject.name + " is missing a language"));
                }

                foreach (var translation in item._translation)
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