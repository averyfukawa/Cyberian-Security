using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EmailListing : MonoBehaviour
{
    public TabInfo tabInfo;
    [Range(1,5)] public int difficultyValue;
    public CaseStatus currentStatus = CaseStatus.Unopened;
    
    // 
    public int caseNumber; // a 1 indexed int
    
    // development id
    public int listingPosition = 0; 
    public string caseName;
    public Tab linkedTab;

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
        if (tabInfo.tabHeadText == "")
        {
            tabInfo.tabHeadText = "Case - " + caseNumber;
        }

        if (tabInfo.tabURL == "emailCase")
        {
            tabInfo.caseNumber = caseNumber;
            tabInfo.tabURL = BrowserManager.Instance.tabList[0].tabURL +"/case" + caseNumber; 
        }
    }
    
    public void OpenEmail()
    {
        if (linkedTab != null)
            // check if it has a linked tab and switch to it, if it has
        {
            BrowserManager.Instance.SetActiveTab(linkedTab);
        }
        else
        {
            // if not, make a new one based on its info and current state, and link it
            linkedTab = BrowserManager.Instance.NewTab(tabInfo, (int)currentStatus);
            // TODO load the saved information on state 1
            if (currentStatus == CaseStatus.Unopened)
            {
                // change the status if it was unopened, and when you do, refresh
                currentStatus++;
                SetVisuals();
                FilingCabinet.Instance.CreateFolder().LabelFolder(_nameField.text, "Case " + caseNumber, caseNumber, listingPosition);

                if (TutorialManager.Instance._doTutorial &&
                    TutorialManager.Instance.currentState == TutorialManager.TutorialState.EmailTwo)
                {
                    TutorialManager.Instance.AdvanceTutorial();
                }
            }
        }
    }
}

[CustomEditor(typeof(EmailListing))]
public class EmailListingEditor : Editor
{
    private EmailListing currentListing;
    
    
    // sets the email position in the list when its not set.
    public override void OnInspectorGUI()
    {
        EmailListing listing = target as EmailListing;


        if (listing.listingPosition == null || listing.listingPosition == 0)
        {
            Undo.RecordObject(this,"setMissionID");
            listing.listingPosition = Int32.Parse(listing.gameObject.name.Split(' ')[1]);
            PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }

        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh Values"))
        {
            listing.SetVisuals();
        }
        
        
    }
}
