using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Canvas))]
public class CanvasEditor : Editor
{
    private Canvas _tc;
    private GameObject _tcObject;
    private Transform _childs;
    private TextCreator _textCreator;
    private string _textField;
    private string _trueTextField;


    private void OnEnable()
    {
        _tc = (Canvas) target;
        _tcObject = _tc.gameObject;
        _childs = _tcObject.gameObject.GetComponentInChildren<Transform>();


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
        }
    }
    
}