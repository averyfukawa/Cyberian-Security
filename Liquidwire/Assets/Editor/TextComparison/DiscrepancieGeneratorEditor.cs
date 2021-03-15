﻿using System;
using System.Collections.Generic;
using System.IO;
using TextComparison;
using UnityEditor;
using UnityEngine;

namespace Editor.TextComparison
{
    [CustomEditor(typeof(DiscrepanciesGenerator))]
    public class DiscrepancieGeneratorEditor : UnityEditor.Editor
    {
        private static string _path;
        private string _Jsonstring;
        private SerializedProperty _SerialisedList;
        private bool _listLoaded;
        private DiscrepanciesGenerator _discrepanciesGenerator;


        private void OnEnable()
        {
            serializedObject.Update();
            _path = Application.dataPath + "/Scripts/TextComparison/Discrepancies.json";
            _Jsonstring = File.ReadAllText(_path);
            
            _discrepanciesGenerator = target as DiscrepanciesGenerator;

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
                SaveJSON();
            }
        }

        public void SaveJSON()
        {
            // copies the dcGen list.
            List<Discrepancie> dcList = _discrepanciesGenerator.dcList;

            // creates new DcList objec to store JSOn in.
            DcList dcObject = new DcList(dcList);
            
            string json = JsonUtility.ToJson(dcObject);

            File.WriteAllText(_path, json);
        }

        private void OnDestroy()
        {
            SaveJSON();
            _listLoaded = false;
        }
    }
}