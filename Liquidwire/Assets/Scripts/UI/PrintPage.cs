using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HoverOverObject), typeof(Rigidbody))]
public class PrintPage : MonoBehaviour
{
    private Rigidbody _rb;
    private HoverOverObject _hOO;
    [SerializeField] private GameObject _fileButton;
    public int caseNumber;
    // tab id 
    public float caseFileId;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _hOO = GetComponent<HoverOverObject>();
        _hOO.ToggleActive();
        _fileButton.SetActive(false);
    }

    private void Update()
    {
        if (_rb.IsSleeping() && _rb.useGravity)
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _hOO.ToggleActive();
            _hOO.SetOriginPoints();
        }
    }

    public void ToggleButton()
    {
        _fileButton.SetActive(!_fileButton.activeSelf);
    }

    public void FileCase()
    {
        CaseFolder folder = FilingCabinet.Instance.FetchFolderByCase(caseNumber);
        folder.FilePage(this);
        int newIdAmount = 0;
        UnderlineRender UR = folder.GetComponentInChildren<UnderlineRender>();
        foreach (var TG in GetComponentsInChildren<TextCreator>())
        {
            TG.SetText(UR.GetIDCount()+newIdAmount);
            TG.clickText.underLiner = UR;
            TG.clickText.caseFolder = folder;
            foreach (var link in TG.clickText.textField.textInfo.linkInfo)
            {
                if (link.GetLinkID() != "")
                {
                    newIdAmount++;
                }   
            }
        }
        UR.AddPage(newIdAmount);
        _fileButton.SetActive(false);

        if (TutorialManager.Instance._doTutorial &&
            TutorialManager.Instance.currentState == TutorialManager.TutorialState.PrintCase)
        {
            TutorialManager.Instance.AdvanceTutorial();
        }
    }
}
