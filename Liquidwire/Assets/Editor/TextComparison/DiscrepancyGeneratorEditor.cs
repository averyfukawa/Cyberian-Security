using System;
using System.Collections.Generic;
using System.IO;
using Games.TextComparison;
using Games.TextComparison.Artificial_dictionary_scripts;
using Games.TextComparison.Discrepancy_generators;
using TextComparison;
using UnityEditor;
using UnityEngine;

namespace Editor.TextComparison
{
    [CustomEditor(typeof(DiscrepancyGenerator))]
    public class DiscrepancyGeneratorEditor : UnityEditor.Editor
    {
        private static string _path;
        private string _Jsonstring;
        private SerializedProperty _SerialisedList;
        private bool _listLoaded;
        private DiscrepancyGenerator _discrepanciesGenerator;
        private void OnEnable()
        {
            serializedObject.Update();
            _path = Application.dataPath + "/Scripts/Games/TextComparison/Discrepancies.json";
            _Jsonstring = File.ReadAllText(_path);
            
            // get targeted object in order to change script variable
            _discrepanciesGenerator = target as DiscrepancyGenerator;

            _listLoaded = false;
        }

        public override void OnInspectorGUI()
        {
            if (_listLoaded)
            {
                base.OnInspectorGUI();
            }

            if (GUILayout.Button("Load List"))
            {
                _discrepanciesGenerator.dcList = JsonUtility.FromJson<DcList>(_Jsonstring).dcList;
                _listLoaded = true;
            }

            if (GUILayout.Button("Save list"))
            {
                SaveJson();
            }
            
        }
        /// <summary>
        /// Save the added discrepancy to the json file
        /// </summary>
        private void SaveJson()
        {
            foreach (var item in _discrepanciesGenerator.dcList)
            {
                foreach (var dictionary in item.discrepancyDictionary)
                {
                    if (dictionary.difficulty > 10)
                    {
                        dictionary.difficulty = 10;
                    }
                }
            }
            // copies the dcGen list.
            List<Discrepancy> dcList = _discrepanciesGenerator.dcList;

            // creates new DcList object to store JSOn in.
            DcList dcObject = new DcList(dcList);
            
            string json = JsonUtility.ToJson(dcObject);

            File.WriteAllText(_path, json);
        }

        private void OnDestroy()
        {
            SaveJson();
            _listLoaded = false;
        }
    }
}