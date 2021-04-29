using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Printer : MonoBehaviour
{
    public static Printer Instance;
    [SerializeField] private GameObject[] _printPagePrefabs = new GameObject[2];
    [SerializeField] private Transform _initialPrintLocation;
    [SerializeField] private Transform[] _printWaypoints;
    [SerializeField] private float _timePerPrintStep;
    
    [SerializeField] private GameObject soundPrinter;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    // determine cut-off points, create followup pages and starting page, print for each with time delay

    public void Print(Tab currentTab, int caseNumber)
    {
        GameObject newPage =
            Instantiate(_printPagePrefabs[0], _initialPrintLocation.position, _initialPrintLocation.rotation);
        GameObject newPageContent = Instantiate(currentTab._printableChildObject,
            newPage.GetComponentInChildren<Canvas>().transform);
        RectTransform rectTrans = newPageContent.GetComponent<RectTransform>();
        rectTrans.anchorMax = new Vector2(.9f,.9f);
        rectTrans.anchorMin = new Vector2(.1f,.1f);
        rectTrans.SetAll(0);

        // this might nullpoint when used on other objects, probably make a second method for it
        Mask mask = newPageContent.GetComponentInChildren<Mask>();
        mask.gameObject.GetComponent<Image>().enabled = false;
        ScrollRect scrollRect = newPageContent.GetComponentInChildren<ScrollRect>();
        scrollRect.verticalScrollbar.gameObject.SetActive(false);
        scrollRect.enabled = false;
        foreach (var tmpUI in newPageContent.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmpUI.ForceMeshUpdate();
        }
        newPageContent.GetComponent<WebsiteScroller>().UpdateFontSize();
        RectTransform bodyRect = mask.transform.GetChild(0).GetComponent<RectTransform>();
        bodyRect.anchorMax = new Vector2(.9f,.9f) * scrollRect.GetComponent<RectTransform>().anchorMax;
        bodyRect.anchorMin = new Vector2(.1f,.1f) * scrollRect.GetComponent<RectTransform>().anchorMin;
        bodyRect.SetAll(0);
        StartCoroutine(SplitPrintPage(bodyRect.GetComponent<TextMeshProUGUI>(), currentTab, caseNumber));
        

        newPage.GetComponent<PrintPage>().caseNumber = caseNumber;
        currentTab.SetTabID();
        newPage.GetComponent<PrintPage>().caseFileId = currentTab.tabId;
        foreach (var webLink in newPageContent.GetComponentsInChildren<WebLinkText>())
        {
            webLink.RemoveLinksForPrint();
            webLink.enabled = false;
        }

        StartCoroutine(PrintByWaypoints(newPage));
    }
    
    public void PrintLoad(Tab currentTab, int caseNumber)
    {
        GameObject newPage =
            Instantiate(_printPagePrefabs[0], _initialPrintLocation.position, _initialPrintLocation.rotation);
        GameObject newPageContent = Instantiate(currentTab._printableChildObject,
            newPage.GetComponentInChildren<Canvas>().transform);
        RectTransform rectTrans = newPageContent.GetComponent<RectTransform>();
        rectTrans.anchorMax = new Vector2(.9f,.9f);
        rectTrans.anchorMin = new Vector2(.1f,.1f);
        rectTrans.SetAll(0);
        
        

        newPage.GetComponent<PrintPage>().caseNumber = caseNumber;
        currentTab.SetTabID();
        newPage.GetComponent<PrintPage>().caseFileId = currentTab.tabId;
        foreach (var webLink in newPageContent.GetComponentsInChildren<WebLinkText>())
        {
            webLink.RemoveLinksForPrint();
            webLink.enabled = false;
        }
        foreach (var TC in newPage.GetComponentsInChildren<TextCreator>())
        {
            TC.clickText.enabled = true; 
        }
        newPage.GetComponent<PrintPage>().FileCase();
    }

    private IEnumerator SplitPrintPage(TextMeshProUGUI bodyText, Tab currentTab, int caseNumber)
    {
        yield return new WaitForSeconds(.1f);
        TMP_WordInfo[] richTextWords = bodyText.textInfo.wordInfo;
        string plainText = Regex.Replace(bodyText.text, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", "");

        string oldText = bodyText.text;
        bodyText.text = plainText;
        bodyText.ForceMeshUpdate();
        if (bodyText.text.Length >
            bodyText.textInfo.lineInfo[bodyText.textInfo.lineCount - 1].lastCharacterIndex+1)
        {
            TextCreator originalTextCreator = bodyText.GetComponent<TextCreator>();
            string[] lastLineWords = new string[0];
            int offSet = 1;
            while (lastLineWords.Length < 5)
            {
                int plainBreakStartIndex = bodyText.textInfo.lineInfo[bodyText.textInfo.lineCount - offSet].firstCharacterIndex;
                int plainBreakEndIndex = bodyText.textInfo.lineInfo[bodyText.textInfo.lineCount - offSet].lastCharacterIndex;
                string lastLine = bodyText.text.Substring(plainBreakStartIndex, plainBreakEndIndex - plainBreakStartIndex);
                lastLineWords = lastLine.Split(new []{',','.','!',' ','?'}, StringSplitOptions.RemoveEmptyEntries);
                offSet++;
            }
            List<string> richTextWordList = new List<string>();
            
            foreach (var word in richTextWords)
            {
                if (word.characterCount > 0)
                {
                    richTextWordList.Add(word.GetWord());
                }
            }

            int counter = 0;
            int index = 0;
            foreach (var word in richTextWordList)
            {
                if (word == lastLineWords[counter])
                {
                    counter++;
                    Debug.Log("found " + word + " one of " +  (lastLineWords.Length - 1));
                    if (counter == lastLineWords.Length - 1)
                    {
                        break;
                    }
                }
                else
                {
                    counter = 0;
                }

                index++;
            }
            int firstCharOfRichTextFirstWordInLastLineIndex = richTextWords[index - counter].firstCharacterIndex;
            string[] splitRefText = originalTextCreator.discrepancyField.Split('|');
            string[] splitTrueText = originalTextCreator.textfield.Split('|');
            int refTextIndex = 0;
            int refTextBreakIndex = -1;
            for (int i = 0; i < splitRefText.Length; i++)
            {
                refTextIndex += splitRefText[i].Length;
                if (firstCharOfRichTextFirstWordInLastLineIndex > refTextIndex)
                {
                    refTextBreakIndex = i - 1;
                }
            }

            string pageOneRefText = "";
            string pageTwoRefText = "";
            string pageOneTrueText = "";
            string pageTwoTrueText = "";
            for (int i = 0; i < splitRefText.Length; i++)
            {
                if (i <= refTextBreakIndex)
                {
                    pageOneRefText += splitRefText[i];
                    pageOneTrueText += splitTrueText[i];
                }
                else
                {
                    pageTwoRefText += splitRefText[i];
                    pageTwoTrueText += splitTrueText[i];
                }
            }
            GameObject newPageTwo = Instantiate(_printPagePrefabs[1], _initialPrintLocation.position, _initialPrintLocation.rotation);
            TextCreator newTextCreator = newPageTwo.GetComponentInChildren<TextCreator>();
            originalTextCreator.discrepancyField = pageOneRefText;
            originalTextCreator.textfield = pageOneTrueText;
            originalTextCreator.SetAnswers(originalTextCreator.discrepancyField);
            newTextCreator.discrepancyField = pageTwoRefText;
            newTextCreator.textfield = pageTwoTrueText;
            newTextCreator.SetAnswers(newTextCreator.discrepancyField);

            newPageTwo.GetComponent<PrintPage>().caseNumber = caseNumber;
            currentTab.SetTabID();
            newPageTwo.GetComponent<PrintPage>().caseFileId = currentTab.tabId;

            bodyText.text = oldText;
            newTextCreator.GetComponent<TextMeshProUGUI>().text =
                oldText.Substring(firstCharOfRichTextFirstWordInLastLineIndex);
            
            yield return new WaitForSeconds(2f);
            StartCoroutine(PrintByWaypoints(newPageTwo));
        }
    }

    private IEnumerator PrintByWaypoints(GameObject pageObject)
    {
        int moveStep = 0;
        
        soundPrinter.GetComponent<AudioOcclusion>().playsFromStart = true;
        soundPrinter.GetComponent<AudioOcclusion>().isPlaying = false;
        while (moveStep < _printWaypoints.Length)
        {
            if (moveStep + 1 != _printWaypoints.Length)
            {
                pageObject.LeanMove(_printWaypoints[moveStep].position, _timePerPrintStep / 2);
                yield return new WaitForSeconds(_timePerPrintStep);
            }
            else
            {
                pageObject.LeanMove(_printWaypoints[moveStep].position, _timePerPrintStep * 1.5f);
            }
            moveStep++;
        }

        foreach (var TC in pageObject.GetComponentsInChildren<TextCreator>())
        {
            TC.clickText.enabled = true;
        }
        pageObject.GetComponent<Rigidbody>().isKinematic = false;
        pageObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
