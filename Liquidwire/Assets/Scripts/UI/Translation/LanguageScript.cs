using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace UI.Translation
{
    public class LanguageScript : MonoBehaviour
    {
        public enum Languages
        {
            Nederlands,
            English
        }

        public Languages currentLanguage;
        public List<ArtificialDictionaryWarning> selectionList = new List<ArtificialDictionaryWarning>();

        public int LanguageNumber()
        {
            return System.Enum.GetValues(typeof(Languages)).Length;
        }

    }
    
    [CustomEditor(typeof(LanguageScript))]
    public class LanguageEditor : UnityEditor.Editor
    {
        private LanguageScript ls;
        private List<string> tempStore = new List<string>();
        private TextMeshProUGUI[] tmpArr;
        private string[] enumList;
        private List<GameObject> prefabObject = new List<GameObject>();
        private List<ArtificialDictionaryStoring> storeList = new List<ArtificialDictionaryStoring>();
        
        private void OnEnable()
        {
            ls = target as LanguageScript;
            enumList = LanguageScript.Languages.GetNames(typeof(LanguageScript.Languages));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Load list"))
            {
                ls.selectionList = new List<ArtificialDictionaryWarning>();
                LoadList();
            }
            if (GUILayout.Button("Attach all Language scripts"))
            {
                List<TextMeshProUGUI> prefabTMP = GetListTMP(prefabObject);
                var temp = tmpArr.Concat(prefabTMP.ToArray()).ToArray();
                InsertComponent(temp);
            }
        }

        private void LoadList()
        {
            var prefabPaths = GetPaths(); 
            prefabObject = OpenPrefabs(prefabPaths);
            tmpArr = FindObjectsOfType<TextMeshProUGUI>(); 
            GetListTMP(prefabObject);
            
            foreach (var prefab in storeList)
            {
                foreach (var names in prefab.names1)
                {
                    ls.selectionList.Add(new ArtificialDictionaryWarning(prefab.prefab1.name + " | " + names, prefab.prefab1));
                }
            }
            foreach (var tmp in tmpArr)
            {
                ls.selectionList.Add(new ArtificialDictionaryWarning(tmp.gameObject.name, tmp.gameObject));
            }
            ls.selectionList = ls.selectionList.OrderBy(s => s.GetObject().name).ToList();
        }

        private void OnDestroy()
        {
            /***
             * Get all the components with the translation scripts when you exit the inspector. Then check if all
             * translation scripts have a translation for every language. if it doesn't it will throw a warning.
             */
            enumList = LanguageScript.Languages.GetNames(typeof(LanguageScript.Languages));
            var list = FindObjectsOfType<TranslationScript>();
            foreach (var currentObject in ls.selectionList)
            {
                if (currentObject.allowWarning)
                {
                    if (currentObject.GetObject().TryGetComponent(out TranslationScript ts))
                    {
                        Debug.Log(currentObject.objectName);
                        if (ts._translation.Count < enumList.Length)
                        {
                            Debug.LogWarning(new Exception(currentObject.objectName + " is missing a language"));
                        }
                        foreach (var translation in ts._translation)
                        {
                            if (String.IsNullOrEmpty(translation.translation))
                            {
                                Debug.LogWarning(new Exception(currentObject.objectName + " is missing a translation"));
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning(new Exception(currentObject.objectName + " has no translations"));
                    }
                }
            }
            // foreach (var item in list)
            // {
            //     ThrowWarnings(item);
            // }
        }

        private void ThrowWarnings(TranslationScript ts)
        {
            if (ts._translation.Count < enumList.Length)
            {
                Debug.LogWarning(new Exception(ts.gameObject.transform.parent.name + " is missing a language"));
            }
            foreach (var translation in ts._translation)
            {
                if (String.IsNullOrEmpty(translation.translation))
                {
                    Debug.LogWarning(new Exception(ts.transform.parent.gameObject.name + " is missing a translation"));
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
                List<string> storeName = new List<string>();
                
                foreach (var item in temp)
                {
                    prefabTMP.Add(item);
                    tempStore.Add(gameObject.name);
                    storeName.Add(item.gameObject.name);
                }
                storeList.Add(new ArtificialDictionaryStoring(gameObject, storeName));
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
