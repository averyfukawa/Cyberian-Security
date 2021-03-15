using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EmailListing : MonoBehaviour
{
    public GameObject caseTabObject;
    public GameObject conclusionTabObject;
    public TabInfo tabInfo;
    [Range(1,5)] public int difficultyValue;
    public CaseStatus currentStatus = CaseStatus.Unopened;
    public int caseNumber; // a 1 indexed int pls
    public string caseName;

    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private TextMeshProUGUI _statusField;
    [SerializeField] private Image[] _difficultyIndicators;
    [SerializeField] private Color[] _difficultyColours = new Color[4];
    
    public enum CaseStatus
    {
        Unopened,
        Started,
        Conclusion
    }

    public void SetVisuals()
    {
        Color diffColour = _difficultyColours[Mathf.FloorToInt((float) (difficultyValue-1) / 2)];
        for (int i = 0; i < 5; i++)
        {
            if (i + 1 <= difficultyValue)
            {
                _difficultyIndicators[i].color = diffColour;
            }
            else
            {
                _difficultyIndicators[i].color = _difficultyColours[_difficultyColours.Length-1];
            }
        }

        _nameField.text = "Case " + caseNumber + " " + caseName;
        _statusField.text = currentStatus.ToString();
        tabInfo = new TabInfo("DMail - "+"Case " + caseNumber, "https://mail.detective.com/mail/u/0/#inbox/case"+ caseNumber, true);
    }

    
}

[CustomEditor(typeof(EmailListing))]
public class EmailListingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh Values"))
        {
            EmailListing listing = target as EmailListing;
            listing.SetVisuals();
        }
    }
}
