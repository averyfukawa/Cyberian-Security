using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CaseFolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _folderLabels = new TextMeshProUGUI[2];
    [SerializeField] private Transform _documentPosition;
    [SerializeField] private Transform _fileWaypoint;
    [SerializeField] private GameObject[] _navigationButtons = new GameObject[2];
    [SerializeField] private Image _labelHidingMask;
    private Rigidbody rb;
    private bool[] _buttonState = new bool[2];
    public Queue<PrintPage> pages = new Queue<PrintPage>();
    private List<PrintPage> pagesL = new List<PrintPage>();
    public int caseNumber;

    private void Start()
    {
        ToggleButtons(false);
        rb = GetComponent<Rigidbody>();
        if (_labelHidingMask != null)
        {
            _labelHidingMask.enabled = false;
        }
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

    public void LabelFolder(string filingLabel, string frontLabel, int _caseNumber)
    {
        _folderLabels[0].text = filingLabel;
        _folderLabels[1].text = frontLabel;
        caseNumber = _caseNumber;
    }

    public void ToggleButtons(bool enable)
    {
        if (enable)
        {
            for (int i = 0; i < 2; i++)
            {
                _navigationButtons[i].SetActive(_buttonState[i]);
            }
        }
        else
        {
            EvaluateButtons();
            foreach (var button in _navigationButtons)
            {
                button.SetActive(false);
            }
        }
    }
    
    private void EvaluateButtons()
    {
        for (var i = 0; i < 2; i++)
        {
            _buttonState[i] = _navigationButtons[i].activeSelf;
        }
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
    }

    private IEnumerator PageFlipAnimationForwards(Transform oldPageTransform, float animationTime)
    {
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
}
