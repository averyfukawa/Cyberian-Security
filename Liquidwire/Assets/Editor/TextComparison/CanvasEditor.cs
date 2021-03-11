using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
[System.Serializable]
[CustomEditor(typeof(Canvas))]

public class CanvasEditor : Editor
{
    private Canvas _tc;
    private GameObject _tcObject;
    private Transform _childs;
    private TextCreator _textCreator;

    [SerializeField] private string _textField;
    [SerializeField] private string _trueTextField;


    private void OnEnable()
    {
        _tc = (Canvas) target;
        _tcObject = _tc.gameObject;
        _childs = _tcObject.gameObject.GetComponentInChildren<Transform>();

        _textField = GetPref("TextField");
        _trueTextField = GetPref("TrueText");
            
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
        GUILayout.Label("TrueTextField");
        _trueTextField = GUILayout.TextArea(_trueTextField);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Set text"))
        {
            _textCreator.textfield = _textField;
            _textCreator.trueTextField = _trueTextField;
            SetPrefs();
        }
    }

    private void OnDestroy()
    {
        EditorPrefs.SetString("TrueText", _trueTextField);
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
        EditorPrefs.SetString("TrueText", _trueTextField);
        EditorPrefs.SetString("TextField", _textField);

    }
}
