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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Attach all Language scripts"))
            {
                /***
                 * Look for all the existing TMP objects and then start the assigning process for the prefabs.
                 * Then loop through them all and try and get the component Translation script if it doesn't
                 * have one already it will add it.
                 */
                var list = FindObjectsOfType<TextMeshProUGUI>();
                Undo.RecordObjects(list, "Assign Translation Objects");
                foreach (var item in list)
                {
                    GameObject temp = item.gameObject;
                    LanguageScript ls = target as LanguageScript;
                    if (!temp.TryGetComponent(out TranslationScript ts))
                    {
                        TranslationScript tempClass = temp.AddComponent<TranslationScript>();
                        tempClass._translation = new List<TranslationObject>();
                        for (int i = 0; i < ls.LanguageNumber(); i++)
                        {
                            tempClass._translation.Add(new TranslationObject("", (LanguageScript.Languages)i));
                        }
                    }
                    PrefabUtility.RecordPrefabInstancePropertyModifications(item);
                }
            }
        }

        private void OnDestroy()
        {
            /***
             * Get all the components with the translation scripts when you exit the inspector. Then check if all
             * translation scripts have a translation for every language. if it doesn't it will throw a warning.
             */
            var enumList = LanguageScript.Languages.GetNames(typeof(LanguageScript.Languages));
            var list = FindObjectsOfType<TranslationScript>();
            
            foreach (var item in list)
            {
                if (item._translation.Count < enumList.Length)
                {
                    Debug.LogWarning(new Exception(item.gameObject.transform.parent.name + " is missing a language"));
                }

                foreach (var translation in item._translation)
                {
                    if (String.IsNullOrEmpty(translation.translation))
                    {
                        Debug.LogWarning(new Exception(item.transform.parent.gameObject.name + " is missing a translation"));
                    }
                }
            }
        }
    }
}