using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor.UI
{
    [CustomEditor(typeof(LanguageScript))]
    public class LanguageEditor : UnityEditor.Editor
    {
        private List<String> tempStore = new List<string>();
        private void OnEnable()
        {
            var prefabPaths = GetPaths();
            List<GameObject> prefabObject = OpenPrefabs(prefabPaths);
            
            List<TextMeshProUGUI> prefabTMP = GetListTMP(prefabObject);
            var arr = FindObjectsOfType<TextMeshProUGUI>();
            var temp = arr.Concat(prefabTMP.ToArray()).ToArray();
            foreach (var VARIABLE in tempStore)
            {
                Debug.Log(VARIABLE + " is one");
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Attach all Language scripts"))
            {
                var prefabPaths = GetPaths();
                List<GameObject> prefabObject = OpenPrefabs(prefabPaths);
                List<TextMeshProUGUI> prefabTMP = GetListTMP(prefabObject);
                
                var arr = FindObjectsOfType<TextMeshProUGUI>();
                var temp = arr.Concat(prefabTMP.ToArray()).ToArray();
                InsertComponent(temp);
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
        /// <summary>
        /// Look for all the existing TMP objects and then start the assigning process for the prefabs.Then loop through
        /// them all and try and get the component Translation script if it doesn't
        /// have one already it will add it.
        /// </summary>
        /// <param name="list"></param>
        private void InsertComponent(TextMeshProUGUI[] list)
        {
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
                    Debug.Log(temp.name + " got added");
                }
                PrefabUtility.RecordPrefabInstancePropertyModifications(item);
            }
        }

        /// <summary>
        /// Get a list of paths to all the prefabs.
        /// </summary>
        /// <returns></returns>
        public string[] GetPaths()
        {
            string[] directory = AssetDatabase.GetAllAssetPaths();
            List<string> result = new List<string>();
            foreach (var item in directory)
            {
                if (item.Contains(".prefab"))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get the list of GameObjects with a TMPUGUI component
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<TextMeshProUGUI> GetListTMP(List<GameObject> list)
        {
            List<TextMeshProUGUI> prefabTMP = new List<TextMeshProUGUI>();
            foreach (var gameObject in list)
            {
                var temp = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var item in temp)
                {
                    prefabTMP.Add(item);
                    tempStore.Add(gameObject.name);
                }
            }
            return prefabTMP;
        }

        /// <summary>
        /// Finds all prefabs and return them
        /// </summary>
        /// <param name="prefabPaths"></param>
        /// <returns></returns>
        private List<GameObject> OpenPrefabs(string[] prefabPaths)
        {
            List<GameObject> prefabObject = new List<GameObject>();
            foreach (var prefab in prefabPaths)
            {
                UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
                GameObject go;
                try {
                    go = (GameObject) o;
                    prefabObject.Add(go);
                } catch {
                    Debug.Log( "For some reason, prefab " + prefab + " won't cast to GameObject" );
                }
            }

            return prefabObject;
        }
    }
}