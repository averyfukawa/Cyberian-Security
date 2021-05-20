﻿using System.Collections;
using System.Collections.Generic;
using Games.TextComparison.Selectable_scripts;
using UI.Browser.Tabs;
using UnityEngine;
using UnityEngine.UI;

public class Printer : MonoBehaviour
{
    public static Printer Instance;
    [SerializeField] private GameObject _printPagePrefab;
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

    #region Printing methods

    /// <summary>
    /// Prints the chosen tab.
    /// </summary>
    /// <param name="currentTab"></param>
    /// <param name="caseNumber"></param>
    public void Print(Tab currentTab, int caseNumber)
    {
        GameObject newPage =
            Instantiate(_printPagePrefab, _initialPrintLocation.position, _initialPrintLocation.rotation);
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

        StartCoroutine(PrintByWaypoints(newPage));
    }
    /// <summary>
    /// Print function for when the player loads a save.
    /// </summary>
    /// <param name="currentTab"></param>
    /// <param name="caseNumber"></param>
    public void PrintLoad(Tab currentTab, int caseNumber)
    {
        GameObject newPage =
            Instantiate(_printPagePrefab, _initialPrintLocation.position, _initialPrintLocation.rotation);
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
        foreach (var tc in newPage.GetComponentsInChildren<TextCreator>())
        {
            tc.clickText.enabled = true; 
        }
        newPage.GetComponent<PrintPage>().FileCase();
    }

    #endregion
    

    /// <summary>
    /// Will make the page that is being printed follow these waypoints and make the printing process take a little longer.
    /// </summary>
    /// <param name="pageObject"></param>
    /// <returns></returns>
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

        foreach (var tc in pageObject.GetComponentsInChildren<TextCreator>())
        {
            tc.clickText.enabled = true;
        }
        pageObject.GetComponent<Rigidbody>().isKinematic = false;
        pageObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
