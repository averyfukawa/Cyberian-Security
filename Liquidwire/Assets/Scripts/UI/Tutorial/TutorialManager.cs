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

    public enum TutorialState
    {
        Opening, // monologue explaining the premise
        Standup, // movement tutorial
        InterimOne, // stand up animation and wait transition
        HelpfolderOne, // monologue explaining the concept of the help folder
        HelpfolderTwo, // monologue explaining the function of the help folder
        HelpfolderThree, // prompt to try and sticky note tutorial
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
        // TODO add language options and text here
        StartCoroutine(MonologueAndWaitAdvance(monologueVisualizer.VisualizeText("intro monologue here")));
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
                // TODO add language options and text here
                monologueVisualizer.VisualizeText("help folder concept explanation here");
                FindObjectOfType<HelpStickyManager>().transform.GetComponentInParent<HelpFolder>().highlight.SetActive(true);
                break;
            case TutorialState.HelpfolderTwo:
                // TODO add language options and text here
                StartCoroutine(MonologueAndWaitAdvance(monologueVisualizer.VisualizeText("help folder functionality explanation here")));
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
}
