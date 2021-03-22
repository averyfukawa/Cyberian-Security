using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace FraudMessage
{
    public class SaveScenario
    {
        private static string _path = Application.dataPath + "/Scripts/FraudMessage/ScenarioSave.json";
        private string _Jsonstring = File.ReadAllText(_path);


        public void Save(ScenarioManager scenarioManager)
        {
        }

        public Scenario Load()
        {
            // editor load all
            Scenario scn = JsonUtility.FromJson<Scenario>(_Jsonstring);
            return scn;
        }
    }
}