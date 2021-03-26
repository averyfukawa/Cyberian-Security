using System.Collections;
using System.Collections.Generic;
using AwesomeToon;
using UnityEditor;
using UnityEngine;

public class ToonShaderRefresher : MonoBehaviour
{
    [SerializeField] private Material _materialToRefresh;
    
    public void RefreshAssignments()
    {
        AwesomeToonHelper[] toonHelpers = FindObjectsOfType<AwesomeToonHelper>();
        foreach (var th in toonHelpers)
        {
            if (th.isMainShader)
            {
                th.material = null;
                th.Init();
                th.material = _materialToRefresh;
                th.Init();
            }
        }
    }
}

[CustomEditor(typeof(ToonShaderRefresher))]
public class ToonShaderRefresherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh Assignments"))
        {
            ToonShaderRefresher refresher = target as ToonShaderRefresher;
            refresher.RefreshAssignments();
        }
    }
}
