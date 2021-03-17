using System.Collections;
using System.Collections.Generic;
using Games.Conversation;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(ScenarioManager))]
public class FraudMessageScenarioEditor : UnityEditor.Editor
{
    // Start is called before the first frame update

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Open editor"))
        {

            FraudMessageScenarioCustomWindow.Open((ScenarioManager) target);
            
        }
    }
}
