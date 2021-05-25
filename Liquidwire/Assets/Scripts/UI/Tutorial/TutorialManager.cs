using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Player.Save_scripts.Save_and_Load_scripts;
using UI.Browser.Emails;
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
                playerObject.GetComponent<Movement>().ChangeLock();
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

                // TODO add language options here
                monologueVisualizer.VisualizeText("Another great feature of D-mail is that it automatically detects which links will be helpful when solving a case, so I don't have to worry about getting sidetracked. \n Any page I can open in an email is relevant in some way, so to keep track of them all I like to print them using the print button in the top left.");
                _reminder = StartCoroutine(DisplayReminderAfterTimer(6f,
                    "To start solving a case I need to print it first because I like to underline suspicious parts. \n It also helps me to avoid rash decisions and I can't accidentally click on dangerous links, which in my line of work is what separates the good detectives from the dead ones..."));
                break;
            case TutorialState.PrintCase:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                
                // TODO add language options here
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f+monologueVisualizer.VisualizeText("The next step is to file the printed pages, so that I don't get them mixed up."),
                    "To leave the computer, simply right click it."));

                break;
            case TutorialState.SolveCaseOne:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }
                
                // TODO add language options here
                FindObjectOfType<CaseFolder>().GetComponent<HelpFolder>().highlight.SetActive(true);
                _reminder = StartCoroutine(DisplayReminderAfterTimer(10f+monologueVisualizer.VisualizeText("Let's look at this new folder. You can find it on the other table."),
                    "I keep the unsolved case folders on the table next to the shelf."));
                break;
            case TutorialState.SolveCaseTwo:
                if (_reminder != null)
                {
                    StopCoroutine(_reminder);
                }

                // TODO add language options here
                monologueVisualizer.VisualizeText("Here I like to mark the parts of emails and websites that make me suspicious of them. \n Either in their wording, the email adresses or in the demands they make of the client. Let's see if we can solve this case after filing all the pages for it !");
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
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText("Thanks to my Detective mail provider, I can view all of my potential cases in an organized manner. \n It seems right now I only have one case however.")+1f);
        _homeTabTutorialObjects[0].SetActive(true);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText("On the left you can see the current status of the case.")+1f);
        _homeTabTutorialObjects[0].SetActive(false);
        _homeTabTutorialObjects[1].SetActive(true);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText("Next to it is the name of the case in my filing system.")+1f);
        _homeTabTutorialObjects[1].SetActive(false);
        _homeTabTutorialObjects[2].SetActive(true);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText("And on the far right you can see the difficulty of the case on a scale of 1 to 5.")+2f);
        _homeTabTutorialObjects[2].SetActive(false);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText("The rest of it works like a normal internet browser with multiple tabs for the various cases.")+1f);
        yield return new WaitForSeconds(monologueVisualizer.VisualizeText("Let's try one right now to demonstrate. \n To start on a case, you simply open the email and start reading, D-mail takes care of the rest !"));
    }

    public void ScoreTutorial(bool hasWon)
    {
        // TODO add language options here
        if (hasWon)
        {
            monologueVisualizer.VisualizeText(
                "Nicely done, let's do a few more, I am usually drowning in work, so there should be new cases...");
            _doTutorial = false;
        }
        else
        {
            monologueVisualizer.VisualizeText(
                "That was not quite all the evidence there was to point to. Some of my clients are really hard to convince, so let's give this another show since this is our first time.");
        }
    }
}
