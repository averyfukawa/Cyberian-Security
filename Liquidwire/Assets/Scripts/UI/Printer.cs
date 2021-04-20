using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Printer : MonoBehaviour
{
    public static Printer Instance;
    [SerializeField] private GameObject _printPagePrefab;
    [SerializeField] private Transform _initialPrintLocation;
    [SerializeField] private Transform[] _printWaypoints;
    [SerializeField] private float _timePerPrintStep;
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
    }

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
        foreach (var TC in newPage.GetComponentsInChildren<TextCreator>())
        {
            TC.clickText.enabled = true; 
        }
        newPage.GetComponent<PrintPage>().FileCase();
    }

    private IEnumerator PrintByWaypoints(GameObject pageObject)
    {
        int moveStep = 0;
        while (moveStep < _printWaypoints.Length)
        {
            pageObject.LeanMove(_printWaypoints[moveStep].position, _timePerPrintStep / 2);
            yield return new WaitForSeconds(_timePerPrintStep);
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
