using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using TMPro;
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
    private List<PrintPage> pagesL = new List<PrintPage>();
    [SerializeField] private GameObject[] winLossPopUps = new GameObject[2];
    public int caseNumber;
    public int caseIndex;
    private bool _solved;
    private SaveCube _saveCube;
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

        _saveCube = FindObjectOfType<SaveCube>();
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

    public List<PrintPage> GetPagesL()
    {
        return pagesL;
    }

    public void LabelFolder(string filingLabel, string frontLabel, int _caseNumber, int _caseIndex)
    {
        _folderLabels[0].text = filingLabel;
        _folderLabels[1].text = frontLabel;
        caseNumber = _caseNumber;
        caseIndex = _caseIndex;
    }

    public void ToggleButtons(bool enable)
    {
        if (enable)
        {
            for (var i = 0; i < _navigationButtons.Length; i++)
            {
                if (i == 2 && !_solved && pages.Count == _saveCube.GetCaseLength(caseIndex)) // TODO prevent duplicate filings of the same page to avoid exploit
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
        GetComponent<TextComparison.AnswerChecker>().FetchAnswerable();
    }

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
        foreach (var CT in oldFrontPage.GetComponentsInChildren<ClickableText>())
        {
            CT.SetInactive();
        }
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
        foreach (var CT in pages.Peek().GetComponentsInChildren<ClickableText>())
        {
            CT.SetActive();
        }
        foreach (var button in _navigationButtons)
        {
            button.SetActive(true);
        }
        inMotion = false;
    }

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
        foreach (var CT in pages.Peek().GetComponentsInChildren<ClickableText>())
        {
            CT.SetActive();
        }
        foreach (var button in _navigationButtons)
        {
            button.SetActive(true);
        }

        inMotion = false;
    }

    private Vector3 FilePositionByIndex(int fileIndex)
    {
        Vector3 basePosition = _documentPosition.position;
        Vector3 offset = -_documentPosition.forward * ((fileIndex - pages.Count) * .0001f);
        return basePosition + offset;
    }

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
    
    public void SortFrontToBack()
    {
        List<PrintPage> pagesT = pages.ToList();
        for (int i = 0; i < pagesT.Count; i++)
        {
            pagesT[i].transform.position = FilePositionByIndex(i);
        }
    }
    
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
        
        _solved = true;
    }
    
    // currently deprecated
    private IEnumerator FadePopup(float time, GameObject textPopup)
    {
        TextMeshProUGUI textMesh = textPopup.GetComponent<TextMeshProUGUI>();
        float timeSpent = 0;
        Color newColour = textMesh.color;
        while (textMesh.color.a > 0)
        {
            timeSpent += Time.deltaTime;
         
            newColour.a = 1 - timeSpent / time;
            textMesh.color = newColour;
            yield return new WaitForEndOfFrame();
        }
        textPopup.SetActive(false);
    }
}
