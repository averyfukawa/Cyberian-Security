using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public TutorialState currentState = TutorialState.Opening;
    public bool _doTutorial { get; private set; }
    private GameObject playerObject;
    public MonologueVisualizer monologueVisualizer;
    private Coroutine _reminder;

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
        EmailThree, // monologue about accepting a case
        EmailFour, // monologue about printing a case
        PrintCase, // monologue about how to file the papers
        SolveCaseOne, // highlight to find the filed case
        SolveCaseTwo, // monologue prompt to solve the case
        SolvedCase // wrap up or retry based on performance
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerObject = FindObjectOfType<PlayerData>().gameObject;
    }

    public void DoTutorial()
    {
        _doTutorial = true;
        // TODO add language options here
        StartCoroutine(MonologueAndWaitAdvance(monologueVisualizer.VisualizeText("In this modern world, people like me are the only thing that stands between helpless consumers and the dangerous hackers who prey on them. \n I am Bram, a private investigator in the field of cyber-crime. Let me walk you through how this works.")));
    }

    public void AdvanceTutorial()
    {
        Debug.Log("advanced tutorial from " + currentState + " to " + (currentState+1));
        currentState++;
        switch (currentState)
        {
            case TutorialState.Standup:
                playerObject.GetComponent<Movement>().changeLock();
                break;
            case TutorialState.InterimOne:
                StartCoroutine(StandUpAndWaitForAdvance(5f));
                break;
            case TutorialState.HelpfolderOne:
                // TODO add language options here
                float monologueLength = monologueVisualizer.VisualizeText("There are many ways to identify fraudulent emails and the like. \n  I have learned them over my long and grueling career, but even I can not remember them all, so I wrote them down in a folder on my desk.");
                FindObjectOfType<HelpStickyManager>().transform.GetComponentInParent<HelpFolder>().highlight.SetActive(true);
                _reminder = StartCoroutine(DisplayReminderAfterTimer(5f+monologueLength,
                    "You can find the folder on the desk with the computer. \n Pick it up using the left mouse button."));
                break;
            case TutorialState.HelpfolderTwo:
                // TODO add language options here
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                StartCoroutine(MonologueAndWaitAdvance(monologueVisualizer.VisualizeText("Another thing I like to do is to highlight ones that I often forget and put them on little sticky notes.")));
                break;
            case TutorialState.HelpfolderThree:
                // TODO add language options here
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f,
                    "To put up a sticky note, simply left click on any help information you want to be reminded of.\n Let's put one up right now !"));
                break;
            case TutorialState.HelpfolderEnd:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }

                // TODO add language options here
                monologueVisualizer.VisualizeText("The next thing to do is to check my emails on the computer.");
                _reminder = StartCoroutine(DisplayReminderAfterTimer(5f,
                    "To put away a folder, simply right click it."));
                break;
            case TutorialState.EmailOne:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }

                // TODO add language options here
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f,
                    "To access the computer, left click it."));
                break;
            case TutorialState.EmailTwo:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }

                break;
        }
    }


    private IEnumerator StandUpAndWaitForAdvance(float waitTimeInSeconds)
    {
        playerObject.transform.LeanMove(transform.position, .5f);
        yield return new WaitForSeconds(.5f);
        playerObject.GetComponent<Movement>().changeLock();
        yield return new WaitForSeconds(waitTimeInSeconds);
        AdvanceTutorial();
    }

    private IEnumerator MonologueAndWaitAdvance(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        AdvanceTutorial();
    }

    private IEnumerator DisplayReminderAfterTimer(float delay, string reminderText)
    {
        yield return new WaitForSeconds(delay);
        monologueVisualizer.VisualizeText(reminderText);
    }
}
