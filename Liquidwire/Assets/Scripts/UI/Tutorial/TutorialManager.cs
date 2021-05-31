using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using Player;
using Player.Save_scripts.Save_and_Load_scripts;
using UI.Browser.Emails;
using UI.Translation;
using UI.Tutorial;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public TutorialState currentState = TutorialState.Opening;
    public bool _doTutorial { get; private set; }
    private GameObject playerObject;
    public MonologueVisualizer monologueVisualizer;
    private Coroutine _reminder;
    private GameObject[] _homeTabTutorialObjects;
    public bool testMode = true;
    private LanguageScript _languageScript;
    private ArtificialDictionaryLanguage _currentLanguage;
    public List<ArtificialDictionaryLanguage> languages = new List<ArtificialDictionaryLanguage>();

    public enum TutorialState
    {
        Opening, // monologue explaining the premise
        Standup, // movement tutorial
        InterimOne, // stand up animation and wait transition
        HelpfolderOne, // monologue explaining the concept of the help folder
        HelpfolderTwo, // monologue explaining the function of the help folder
        HelpfolderThree, // prompt to try and sticky note tutorial
        HelpfolderEnd, // monologue explaining how to close the help folder
        EmailOne, // interim step for additional guidance if needed
        EmailTwo, // monologue and visuals explaining how the email inbox works
        EmailThree, // monologue about reading and printing a case
        PrintCase, // monologue about how to file the papers
        SolveCaseOne, // highlight to find the filed case
        SolveCaseTwo // monologue prompt to solve the case
        // wrap up or retry based on performance
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        FolderMenu.setLanguageEvent += SetLanguage;
        playerObject = FindObjectOfType<PlayerData>().gameObject;
        
        List<GameObject> temp = new List<GameObject>();
        foreach (var image in FindObjectOfType<EmailInbox>().transform.Find("TabBody").GetChild(0).Find("TutorialObjects").GetComponentsInChildren<Image>())
        {
            temp.Add(image.gameObject);
            image.gameObject.SetActive(false);
        }
        _homeTabTutorialObjects = temp.ToArray();

        if (testMode)
        {
            Debug.LogWarning("The tutorial is set to testing mode, this enables developer shortcuts. please disable it before a build");
        }
    }

    private void Update()
    {
        if (testMode && Input.GetKeyDown(KeyCode.P))
        {
            StopAllCoroutines();
            AdvanceTutorial();
        }
    }

    private void SetLanguage()
    {
        _languageScript = FindObjectOfType<LanguageScript>();
        foreach (var language in languages)
        {
            if (language.GetLanguages() == _languageScript.currentLanguage)
            {
                _currentLanguage = language;
            }
        }
    }

    public void DoTutorial()
    {
        _doTutorial = true;
        // TODO add language options here
        StartCoroutine(MonologueAndWaitAdvance(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.StartMonologue))));
    }

    public void AdvanceTutorial()
    {
        Debug.Log("advanced tutorial from " + currentState + " to " + (currentState+1));
        currentState++;
        switch (currentState)
        {
            case TutorialState.Standup:
                playerObject.GetComponent<Movement>().ChangeLock();
                break;
            case TutorialState.InterimOne:
                StartCoroutine(StandUpAndWaitForAdvance(5f));
                break;
            case TutorialState.HelpfolderOne:
                float monologueLength = monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.HelpFolderOneP1));
                FindObjectOfType<HelpStickyManager>().transform.GetComponentInParent<HelpFolder>().highlight.SetActive(true);
                _reminder = StartCoroutine(DisplayReminderAfterTimer(5f+monologueLength,
                    _currentLanguage.GetTextBasedOnPart(TutorialTextPart.HelpFolderOneP2)));
                break;
            case TutorialState.HelpfolderTwo:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                StartCoroutine(MonologueAndWaitAdvance(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.HelpFolderTwo))));
                break;
            case TutorialState.HelpfolderThree:
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f,
                    _currentLanguage.GetTextBasedOnPart(TutorialTextPart.HelpFolderThree)));
                break;
            case TutorialState.HelpfolderEnd:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                
                monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.HelpFolderEndP1));
                _reminder = StartCoroutine(DisplayReminderAfterTimer(5f,
                    _currentLanguage.GetTextBasedOnPart(TutorialTextPart.HelpFolderEndP2)));
                break;
            case TutorialState.EmailOne:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f,
                    _currentLanguage.GetTextBasedOnPart(TutorialTextPart.EmailOne)));
                break;
            case TutorialState.EmailTwo:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                _reminder = StartCoroutine(InboxWalkThrough());
                break;
            case TutorialState.EmailThree:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                    foreach (var obj in _homeTabTutorialObjects)
                    {
                        obj.SetActive(false);
                    }
                }
                monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.EmailThreeP1));
                _reminder = StartCoroutine(DisplayReminderAfterTimer(6f,
                    _currentLanguage.GetTextBasedOnPart(TutorialTextPart.EmailThreeP2)));
                break;
            case TutorialState.PrintCase:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f+monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.PrintCaseP1)),
                    _currentLanguage.GetTextBasedOnPart(TutorialTextPart.PrintCaseP2)));

                break;
            case TutorialState.SolveCaseOne:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                FindObjectOfType<CaseFolder>().GetComponent<HelpFolder>().highlight.SetActive(true);
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f+monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.SolveCaseOneP1)),
                    _currentLanguage.GetTextBasedOnPart(TutorialTextPart.SolveCaseOneP2)));
                break;
            case TutorialState.SolveCaseTwo:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.SolveCaseTwo));
                break;
        }
    }


    private IEnumerator StandUpAndWaitForAdvance(float waitTimeInSeconds)
    {
        playerObject.transform.LeanMove(transform.position, .5f);
        yield return new WaitForSeconds(.5f);
        playerObject.GetComponent<Movement>().ChangeLock();
        yield return new WaitForSeconds(waitTimeInSeconds);
        AdvanceTutorial();
    }

    private IEnumerator MonologueAndWaitAdvance(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime*.9f);
        AdvanceTutorial();
    }

    private IEnumerator DisplayReminderAfterTimer(float delay, string reminderText)
    {
        yield return new WaitForSeconds(delay);
        monologueVisualizer.VisualizeText(reminderText);
    }

    private IEnumerator InboxWalkThrough()
    {
        // TODO add language options here
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.InboxWalkThroughP1))+1f);
        _homeTabTutorialObjects[0].SetActive(true);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.InboxWalkThroughP2))+1f);
        _homeTabTutorialObjects[0].SetActive(false);
        _homeTabTutorialObjects[1].SetActive(true);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.InboxWalkThroughP3))+1f);
        _homeTabTutorialObjects[1].SetActive(false);
        _homeTabTutorialObjects[2].SetActive(true);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.InboxWalkThroughP4))+2f);
        _homeTabTutorialObjects[2].SetActive(false);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.InboxWalkThroughP5))+1f);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText(_currentLanguage.GetTextBasedOnPart(TutorialTextPart.InboxWalkThroughP6)));
    }

    public void ScoreTutorial(bool hasWon)
    {
        // TODO add language options here
        if (hasWon)
        {
            monologueVisualizer.VisualizeText(
                _currentLanguage.GetTextBasedOnPart(TutorialTextPart.TutorialWin));
            _doTutorial = false;
        }
        else
        {
            monologueVisualizer.VisualizeText(
                _currentLanguage.GetTextBasedOnPart(TutorialTextPart.TutorialLose));
        }
    }
}
[UnityEditor.CustomEditor(typeof(TutorialManager))]
public class InspectorCustomizer : UnityEditor.Editor
{
    private TutorialManager _tm;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _tm = target as TutorialManager;
        foreach (var language in _tm.languages)
        {
            language.name = language.GetLanguages().ToString();
            foreach (var text in language.GetTextList())
            {
                text.name = text.GetPart().ToString();
            }
        }
    }
}
