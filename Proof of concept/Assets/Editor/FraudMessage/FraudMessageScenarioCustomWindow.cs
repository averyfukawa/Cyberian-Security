using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class FraudMessageScenarioCustomWindow : ExtendedFMSWindow
{
    // Start is called before the first frame update

    public static void Open(ScenarioManager dataGameObject)
    {
        FraudMessageScenarioCustomWindow window = GetWindow<FraudMessageScenarioCustomWindow>("Fraud message editor ");
        window._serializedObject = new SerializedObject(dataGameObject);
    }


    private void OnGUI()
    {
        
        Drawproperties();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}