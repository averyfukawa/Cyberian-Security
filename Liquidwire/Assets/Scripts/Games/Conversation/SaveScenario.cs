using System.IO;
using UnityEngine;

namespace Games.Conversation
{
    public class SaveScenario
    {
        private static string _path = Application.dataPath + "/Scripts/Games/Conversation/ScenarioSave.json";
        private string _Jsonstring = File.ReadAllText(_path);


        public void Save(Games.Conversation.ScenarioManager scenarioManager)
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