﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(ScenarioManager))]
public class FraudMessageScenarioEditor : Editor
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