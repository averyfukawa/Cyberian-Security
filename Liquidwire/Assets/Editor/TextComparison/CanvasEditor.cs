using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
[System.Serializable]
[CustomEditor(typeof(Canvas))]

public class CanvasEditor : UnityEditor.Editor
{
    private Canvas _tc;
    private GameObject _tcObject;
    private Transform _childs;
    private TextCreator _textCreator;
    private int difficulty;

    [SerializeField] private string _textField;

    private void OnEnable()
    {
        _tc = (Canvas) target;
        _tcObject = _tc.gameObject;
        
        _childs = _tcObject.gameObject.GetComponentInChildren<Transform>();

        _textField = GetPref("TextField");
            
        // find the sentencebutton in order to set the text. 
        foreach (Transform t in _childs)
        {
            if (t.name == "SentenceButton")
            {
                _textCreator = t.gameObject.GetComponent<TextCreator>();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("textField");
        _textField = GUILayout.TextArea(_textField);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Difficulty");

        difficulty = (int) EditorGUILayout.Slider(difficulty , 1, 10);
        GUILayout.EndHorizontal();


        if (GUILayout.Button("Save text & difficulty"))
        {
            _textCreator.textfield = _textField;
            _textCreator.difficulty = difficulty;
            SetPrefs();
        }
    }

    private void OnDestroy()
    {
        EditorPrefs.SetString("TextField", _textField);
    }

    private String GetPref(string key)
    {
        string value = "";
        if (EditorPrefs.HasKey(key))
        {
            value = EditorPrefs.GetString(key);
        }

        return value;
    }

    private void SetPrefs()
    {
        EditorPrefs.SetString("TextField", _textField);

    }
}
