using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HelpStickyManager : MonoBehaviour
{
    public List<HelpStickyObject> objectListByID;

    private int _currentSticky = 0;
    private Camera _mainCamera;
    private bool _isActive = false;
    [SerializeField] private Transform[] stickyPositions;
    public TextMeshProUGUI _helpTextUI; // an element only used in editor to assign correct page lengths
    public List<int> linkPageByID = new List<int>();
    [SerializeField] private GameObject _stickyPrefab;
    [SerializeField] private UnderlineRender _underLiner;
    [SerializeField] private GameObject helpPagePrefab;
    private TMP_LinkInfo[] _linkInfos;
    private HelpPageViewer hpv;

    private void Start()
    {
        // if the help text is not proper, fix in editor pls, there are buttons for that
        hpv = GetComponent<HelpPageViewer>();
        _mainCamera = Camera.main;
        StartCoroutine(FetchTMPInfoAfterDelay());
    }

    private IEnumerator FetchTMPInfoAfterDelay()
    {
        yield return new WaitForEndOfFrame();
        _linkInfos = hpv.FetchLinkInfos();
        _underLiner.Setup(hpv.pagesL.Count, _linkInfos.Length);
        _helpTextUI.transform.parent.gameObject.SetActive(false);
    }

    public void ToggleInteractable()
    {
        _isActive = !_isActive;
        if (_isActive == false)
        {
            _underLiner.DropLines();
        }
        else
        {
            foreach (var obje in objectListByID)
            {
                if (obje.isStickied)
                {
                    int linkId = -1;
                    for (int i = 0; i < _linkInfos.Length; i++)
                    {
                        if (_linkInfos[i].GetLinkText().Equals(obje.helpText))
                        {
                            linkId = i;
                            break;
                        }
                    }
                    
                    _underLiner.CreateLines(CreateUnderlineCoords(linkId), linkPageByID[linkId], linkId);
                }
            }
        }
    }

    public void CreateHelpTextPages()
    {
        HelpPageViewer hpv = GetComponent<HelpPageViewer>();
        Undo.RecordObject(hpv, "Changed Help Pages");
        hpv.EmptyFolder();
        linkPageByID = new List<int>();
        // create temp text
        int counter = 0;
        string newText = "";
        Queue<string> textParts = new Queue<string>();

        foreach (var obje in objectListByID)
        {
            string addedText = "<link=" + counter + ">" + obje.helpText + "</link> " + "\n \n ";
            newText += addedText;
            counter++;
            textParts.Enqueue(addedText);
        }

        _helpTextUI.text =  newText;
        _helpTextUI.ForceMeshUpdate();
        int pageCount = 0;
        // split onto pages
        while (_helpTextUI.text != "")
        {
            TMP_LinkInfo[] links = _helpTextUI.textInfo.linkInfo;
            List<TMP_LinkInfo> linkL = links.ToList();
            linkL.RemoveRange(counter, links.Length-counter);
            links = linkL.ToArray();
            TMP_PageInfo[] pages = _helpTextUI.textInfo.pageInfo;
            for (var index = 0; index < links.Length; index++)
            {
                var link = links[index];
                if (link.linkIdLength > 0)
                {

                    if (link.linkTextfirstCharacterIndex < pages[0].lastCharacterIndex &&
                        link.linkTextfirstCharacterIndex + link.linkTextLength > pages[0].lastCharacterIndex)
                    {
                        // text is stretched over two pages, create a new text for all before this and shorten/update layout
                        GameObject newPage = Instantiate(helpPagePrefab, Vector3.zero, Quaternion.identity);
                        string pageText = "";
                        string compareText = "<link=" + link.GetLinkID() + ">" + link.GetLinkText() + "</link> " +
                                             "\n \n ";
                        while (textParts.Peek() != compareText)
                        {
                            pageText += textParts.Dequeue();
                            linkPageByID.Add(pageCount);
                            counter--;
                        }

                        newPage.GetComponentInChildren<TextMeshProUGUI>().text = pageText;
                        hpv.FilePage(newPage);
                        _helpTextUI.text = _helpTextUI.text.Replace(pageText, "");
                        _helpTextUI.ForceMeshUpdate();
                        pageCount++;
                        PrefabUtility.RecordPrefabInstancePropertyModifications(newPage);
                        break;
                    }

                    if ((int.Parse(link.GetLinkID()) == objectListByID.Count-1) ||
                        (links[index + 1].linkTextfirstCharacterIndex > pages[0].lastCharacterIndex)
                    ) // page just ends peacefully
                    {
                        // create a new text for all including this and shorten/update layout
                        GameObject newPage = Instantiate(helpPagePrefab, Vector3.zero, Quaternion.identity);
                        string pageText = "";
                        string compareText = "<link=" + link.GetLinkID() + ">" + link.GetLinkText() + "</link> " +
                                             "\n \n ";
                        while (textParts.Peek() != compareText)
                        {
                            pageText += textParts.Dequeue();
                            linkPageByID.Add(pageCount);
                            counter--;
                        }

                        pageText += textParts.Dequeue();
                        counter--;

                        newPage.GetComponentInChildren<TextMeshProUGUI>().text = pageText;
                        hpv.FilePage(newPage);
                        _helpTextUI.text = _helpTextUI.text.Replace(pageText, "");
                        _helpTextUI.ForceMeshUpdate();
                        pageCount++;
                        PrefabUtility.RecordPrefabInstancePropertyModifications(newPage);
                        break;
                    }
                }
            }
        }
        hpv.SortFrontToBack();
        PrefabUtility.RecordPrefabInstancePropertyModifications(hpv);
    }

    private void Update()
    {
        //Check if the left mouse button was used to click
        if (Input.GetMouseButtonUp(0) && _isActive)
        {
            //Finds the correct link based on the mouse position and the text field
            TextMeshProUGUI textMesh = hpv.pages.Peek().GetComponentInChildren<TextMeshProUGUI>();
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMesh, Input.mousePosition, _mainCamera);
            if (linkIndex != -1)
            {
                int linkId = int.Parse(textMesh.textInfo.linkInfo[linkIndex].GetLinkID());
                HelpStickyObject currentObj = objectListByID[linkId];
                // reference it against the sticky list to see if it should get stickied or it should get un-stickied
                if (currentObj.isStickied)
                {
                    // un-sticky it
                    currentObj.stickyNote.SetActive(false);
                    currentObj.isStickied = false;
                }
                else
                {
                    // sticky it
                    if (currentObj.stickyNote != null)
                    {
                        currentObj.stickyNote.SetActive(true);
                        currentObj.isStickied = true;
                    }
                    else
                    { // create a new sticky
                        if (_currentSticky == 10)
                        {
                            _currentSticky = 0;
                        }

                        foreach (var obje in objectListByID)
                        {
                            if (obje.stickyID == _currentSticky)
                            {
                                Destroy(obje.stickyNote);
                                obje.stickyID = -1;
                            }
                        }
                        currentObj.stickyNote = Instantiate(_stickyPrefab, stickyPositions[_currentSticky]);
                        currentObj.stickyNote.GetComponentInChildren<TextMeshProUGUI>().text = currentObj.stickyText;
                        currentObj.stickyID = _currentSticky;
                        _currentSticky++;
                        currentObj.isStickied = true;
                    }
                }

                if (currentObj.isStickied)
                {
                    _underLiner.CreateLines(CreateUnderlineCoords(linkId), hpv.CurrentPageNumber(), linkId);
                }
                else
                {
                    _underLiner.DestroyLine(linkId);
                }
            }
        }
    }

    private Vector3[] CreateUnderlineCoords(int linkID)
    { // this method calculates the coordinates used by the line renderer,
      // they are returned in pairs of left, right grouped in an array with a total length of (lines of text in link)*2
        List<Vector3> coords = new List<Vector3>();
        TextMeshProUGUI targetPage = hpv.pages.Peek().GetComponentInChildren<TextMeshProUGUI>();
        TMP_TextInfo textInfo = targetPage.textInfo;

        Vector3 offset = new Vector3(0,-.001f,0);
        TMP_LinkInfo linkInfo = new TMP_LinkInfo();
        foreach (var link in textInfo.linkInfo)
        {
            if (int.Parse(link.GetLinkID()) == linkID)
            {
                linkInfo = link;
                break;
            }
        }
        int startLine = targetPage.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex].lineNumber;
        int endLine = targetPage.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex+linkInfo.linkTextLength].lineNumber;
        Transform textTransform = targetPage.transform;
        int linkStartIndex = linkInfo.linkTextfirstCharacterIndex;
        int linkEndIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength;
        for (int i = startLine; i < endLine+1; i++)
        {
            int startIndex = textInfo.lineInfo[i].firstCharacterIndex > linkStartIndex ? textInfo.lineInfo[i].firstCharacterIndex : linkStartIndex;
            int endIndex = textInfo.lineInfo[i].lastCharacterIndex-3 < linkEndIndex
                ? textInfo.lineInfo[i].lastCharacterIndex-3 : linkEndIndex;
            coords.Add(textTransform.TransformPoint(textInfo.characterInfo[startIndex].bottomLeft) + offset);
            coords.Add(textTransform.TransformPoint(textInfo.characterInfo[endIndex].bottomRight) + offset);
        }
        return coords.ToArray();
    }

    
}

[Serializable] public class HelpStickyObject
{
    public string helpText;
    public string stickyText;
    public bool isStickied;
    public GameObject stickyNote;
    public int stickyID = -1;
}

[CustomEditor(typeof(HelpStickyManager))]
public class StickyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Add Help Object"))
        {
            HelpStickyManager stickyManager = target as HelpStickyManager;
            stickyManager.objectListByID.Add(new HelpStickyObject());
        }
        if (GUILayout.Button("Generate Help Text"))
        {
            HelpStickyManager stickyManager = target as HelpStickyManager;
            stickyManager.CreateHelpTextPages();
        }
    }
}
