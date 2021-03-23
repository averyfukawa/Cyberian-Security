using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaseFolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] _folderLabels = new TextMeshProUGUI[2];
    [SerializeField] private Transform _documentPosition;
    [SerializeField] private GameObject[] _navigationButtons = new GameObject[2];
    private Rigidbody rb;
    private bool[] _buttonState = new bool[2];
    public List<PrintPage> pages = new List<PrintPage>();
    public int caseNumber;

    private void Start()
    {
        ToggleButtons(false);
        rb = GetComponent<Rigidbody>();
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
}
