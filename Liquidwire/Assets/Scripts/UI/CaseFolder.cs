using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.TextComparison;
using Games.TextComparison.Selectable_scripts;
using MissionSystem;
using Player;
using Player.Raycasting;
using Player.Save_scripts.Save_system_interaction;
using TMPro;
using UI;
using UI.Browser;
using UI.Browser.Emails;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CaseFolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _folderLabels = new TextMeshProUGUI[2];
    [SerializeField] private Transform _documentPosition;
    [SerializeField] private Transform _fileWaypoint;
    [SerializeField] private GameObject[] _navigationButtons = new GameObject[3];
    [SerializeField] private Image _labelHidingMask;
    private Rigidbody rb;
    public Queue<PrintPage> pages = new Queue<PrintPage>();
    /// <summary>
    /// A list with all the pages inside the folder
    /// </summary>
    private List<PrintPage> pagesL = new List<PrintPage>();
    /// <summary>
    /// A list with the different outcome components
    /// </summary>
    [SerializeField] private GameObject[] winLossPopUps = new GameObject[2];
    public int caseNumber;
    public int caseIndex;
    /// <summary>
    /// If the case has been solved or not.
    /// </summary>
    private bool _solved;
    private SaveManager _saveManager;
    /// <summary>
    /// If the folder is still in motion it will not be interatable.
    /// </summary>
    public bool inMotion;

    private void Start()
    {
        ToggleButtons(false);
        rb = GetComponent<Rigidbody>();
        if (_labelHidingMask != null)
        {
            _labelHidingMask.enabled = false;
        }

        foreach (var popUp in winLossPopUps)
        {
            popUp.SetActive(false);
        }

        _saveManager = FindObjectOfType<SaveManager>();
    }

    private void Update()
    {
        if (rb.IsSleeping() && rb.useGravity)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            GetComponent<HoverOverObject>().SetOriginPoints();
        }
    }

    #region Page animations
    
    /// <summary>
    /// Flips the page according to the bool provided
    /// </summary>
    /// <param name="forwards"></param>
    public void FlipPage(bool forwards)
    {
        PrintPage oldFrontPage;
        if (forwards)
        {
            oldFrontPage = pages.Dequeue();
            pages.Enqueue(oldFrontPage);
            StartCoroutine(PageFlipAnimationForwards(oldFrontPage.transform, 0.5f));
        }
        else
        {
            PrintPage[] tempArray = pages.ToArray();
            oldFrontPage = pages.Peek();
            PrintPage tempValue = tempArray[pages.Count - 1];
            for (int i = 0; i < tempArray.Length; i++)
            {
                PrintPage temp = tempArray[i];
                tempArray[i] = tempValue;
                tempValue = temp;
            }
            pages = new Queue<PrintPage>(tempArray);
            StartCoroutine(PageFlipAnimationBackwards(oldFrontPage.transform, 0.5f));
        }
        oldFrontPage.GetComponentInChildren<UnderlineRender>().DropLines();
        foreach (var ct in oldFrontPage.GetComponentsInChildren<ClickableText>())
        {
            ct.SetInactive();
        }
    }
    
    /// <summary>
    /// Flips the page to the back to the front
    /// </summary>
    /// <param name="oldPageTransform"></param>
    /// <param name="animationTime"></param>
    /// <returns></returns>
    private IEnumerator PageFlipAnimationBackwards(Transform oldPageTransform, float animationTime)
    {
        inMotion = true;
        foreach (var button in _navigationButtons)
        {
            button.SetActive(false);
        }
        oldPageTransform.LeanMove(FilePositionByIndex(1), animationTime*.2f);
        pages.Peek().transform.LeanMove(_fileWaypoint.position, animationTime*.5f);
        yield return new WaitForSeconds(animationTime*.1f);
        _labelHidingMask.enabled = true;
        yield return new WaitForSeconds(animationTime*.4f);
        pages.Peek().transform.LeanMove(FilePositionByIndex(0), animationTime*.5f);
        yield return new WaitForSeconds(animationTime*.4f);
        _labelHidingMask.enabled = false;
        yield return new WaitForSeconds(animationTime*.1f);
        foreach (var ct in pages.Peek().GetComponentsInChildren<ClickableText>())
        {
            ct.SetActive();
        }
        
        for (var i = 0; i < _navigationButtons.Length; i++)
        {
            if (i == 2 && !_solved && pages.Count == _saveManager.GetCaseLength(caseIndex))
            {
                _navigationButtons[i].SetActive(true); 
            }
            else if(i != 2)
            {
                _navigationButtons[i].SetActive(true); 
            }
        }
        inMotion = false;
    }

    /// <summary>
    /// Flips the page to the front to the back
    /// </summary>
    /// <param name="oldPageTransform"></param>
    /// <param name="animationTime"></param>
    /// <returns></returns>
    private IEnumerator PageFlipAnimationForwards(Transform oldPageTransform, float animationTime)
    {
        inMotion = true;
        foreach (var button in _navigationButtons)
        {
            button.SetActive(false);
        }
        pages.Peek().transform.LeanMove(FilePositionByIndex(0), animationTime*.2f);
        oldPageTransform.LeanMove(_fileWaypoint.position, animationTime*.5f);
        yield return new WaitForSeconds(animationTime*.1f);
        _labelHidingMask.enabled = true;
        yield return new WaitForSeconds(animationTime*.4f);
        oldPageTransform.LeanMove(FilePositionByIndex(pages.Count-1), animationTime*.5f);
        yield return new WaitForSeconds(animationTime*.4f);
        _labelHidingMask.enabled = false;
        yield return new WaitForSeconds(animationTime*.1f);
        foreach (var ct in pages.Peek().GetComponentsInChildren<ClickableText>())
        {
            ct.SetActive();
        }
        for (var i = 0; i < _navigationButtons.Length; i++)
        {
            if (i == 2 && !_solved && pages.Count == _saveManager.GetCaseLength(caseIndex))
            {
                _navigationButtons[i].SetActive(true); 
            }
            else if(i != 2)
            {
                _navigationButtons[i].SetActive(true); 
            }
        }

        inMotion = false;
    }

    #endregion
    
    #region Filing

    /// <summary>
    /// Files the page provided into the current casefolder.
    /// </summary>
    /// <param name="pageToFile"></param>
    public void FilePage(PrintPage pageToFile)
    {
        pages.Enqueue(pageToFile);
        pagesL.Add(pageToFile);
        var transform1 = pageToFile.transform;
        transform1.SetParent(_documentPosition, true);
        HoverOverObject hoo = pageToFile.GetComponent<HoverOverObject>();
        hoo.ForceQuitInspect();
        hoo.ToggleActive();
        transform1.position = FilePositionByIndex(pages.Count);
        transform1.localRotation = Quaternion.Euler(new Vector3(0,0,Random.Range(-5f, 5f)));
        SortFrontToBack();
    }
    
    /// <summary>
    /// Sorts the pages
    /// </summary>
    public void SortFrontToBack()
    {
        List<PrintPage> pagesT = pages.ToList();
        for (int i = 0; i < pagesT.Count; i++)
        {
            pagesT[i].transform.position = FilePositionByIndex(i);
        }
    }

    #endregion

    #region Getters

    public List<PrintPage> GetPagesL()
    {
        return pagesL;
    }

    public int CurrentPageNumber()
    {
        for (int i = 0; i < pagesL.Count; i++)
        {
            if (pagesL[i] == pages.Peek())
            {
                return i;
            }
        }
        return -1;
    }
    
    #endregion
    
    /// <summary>
    /// Puts the Label on top of the folder
    /// </summary>
    /// <param name="filingLabel"></param>
    /// <param name="frontLabel"></param>
    /// <param name="_caseNumber"></param>
    /// <param name="_caseIndex"></param>
    public void LabelFolder(string filingLabel, string frontLabel, int _caseNumber, int _caseIndex)
    {
        _folderLabels[0].text = filingLabel;
        _folderLabels[1].text = frontLabel;
        caseNumber = _caseNumber;
        caseIndex = _caseIndex;
    }

    /// <summary>
    /// Toggles the buttons to navigate the folder
    /// </summary>
    /// <param name="enable"></param>
    public void ToggleButtons(bool enable)
    {
        if (enable)
        {
            for (var i = 0; i < _navigationButtons.Length; i++)
            {
                if (i == 2 && !_solved && pages.Count == _saveManager.GetCaseLength(caseIndex))
                {
                    _navigationButtons[i].SetActive(true); 
                }
                else if(i != 2)
                {
                    _navigationButtons[i].SetActive(true); 
                }
            }
        }
        else
        {
            foreach (var button in _navigationButtons)
            {
                button.SetActive(false);
            }
        }
        GetComponent<AnswerChecker>().FetchAnswerable();
    }


    /// <summary>
    /// Gets the transform position plus the offset
    /// </summary>
    /// <param name="fileIndex"></param>
    /// <returns></returns>
    private Vector3 FilePositionByIndex(int fileIndex)
    {
        Vector3 basePosition = _documentPosition.position;
        Vector3 offset = -_documentPosition.forward * ((fileIndex - pages.Count) * .0001f);
        return basePosition + offset;
    }
    
    /// <summary>
    /// Displays the outcome of the case.
    /// </summary>
    /// <param name="hasWon"></param>
    public void DisplayOutcome(bool hasWon)
    {
        if (TutorialManager.Instance._doTutorial &&
            TutorialManager.Instance.currentState == TutorialManager.TutorialState.SolveCaseTwo)
        {
            if (hasWon)
            {
                TutorialManager.Instance.ScoreTutorial(true);
            }
            else
            {
                TutorialManager.Instance.ScoreTutorial(false);
                return;
            }
        }
        
        if (hasWon)
        {
            winLossPopUps[0].SetActive(true);
            winLossPopUps[1].SetActive(false);
        }
        else
        {
            winLossPopUps[1].SetActive(true);
            winLossPopUps[0].SetActive(false);
        }

        MissionManager misMan = FindObjectOfType<MissionManager>();
        misMan.FindAndAddMission();

        EmailInbox inBox = FindObjectOfType<EmailInbox>();
        foreach (var email in inBox.GetEmails())
        {
            if (email.caseNumber == caseNumber)
            {
                email.currentStatus = EmailListing.CaseStatus.Conclusion;
                email.SetVisuals();
                break;
            }
        }

        for (var index = 0; index < BrowserManager.Instance.tabList.Count; index++)
        {
            var tab = BrowserManager.Instance.tabList[index];
            if (tab.caseNumber == caseNumber)
            {
                BrowserManager.Instance.CloseTab(tab);
            }
        }

        _solved = true;
    }
}
