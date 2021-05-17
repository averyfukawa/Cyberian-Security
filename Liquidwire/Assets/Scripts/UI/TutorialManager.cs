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

    public enum TutorialState
    {
        Opening,
        Standup,
        InterimOne,
        Helpfolder,
        
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
        // TODO start monologue 1 and remove this advance to be put at the end of it
        AdvanceTutorial();
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
}
