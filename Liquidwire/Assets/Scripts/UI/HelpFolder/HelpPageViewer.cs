using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpPageViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField] private GameObject[] _pageButtons = new GameObject[2];
    [SerializeField] private UnderlineRender _underLiner;
    private bool[] _buttonState = new bool[2];

    private void Start()
    {
        _pageButtons[1].SetActive(false);
        ToggleButtons(false);
    }

    public void ToggleButtons(bool enable)
    {
        if (enable)
        {
            for (int i = 0; i < 2; i++)
            {
                _pageButtons[i].SetActive(_buttonState[i]);
            }
        }
        else
        {
            EvaluateButtons();
            foreach (var button in _pageButtons)
            {
                button.SetActive(false);
            }
        }
    }

    private void EvaluateButtons()
    {
        for (var i = 0; i < 2; i++)
        {
            _buttonState[i] = _pageButtons[i].activeSelf;
        }
    }

    public void ChangePage(bool isForward)
    {
        if (isForward)
        {
            _pageText.pageToDisplay++;
            if (_pageText.pageToDisplay == _pageText.textInfo.pageCount)
            {
                _pageButtons[0].SetActive(false);
            }
            if (!_pageButtons[1].activeSelf)
            {
                _pageButtons[1].SetActive(true);
            }
            _underLiner.MovePage(true);
        }
        else
        {
            _pageText.pageToDisplay--;
            if (_pageText.pageToDisplay < 2)
            {
                _pageButtons[1].SetActive(false);
            }
            if (!_pageButtons[0].activeSelf)
            {
                _pageButtons[0].SetActive(true);
            }
            _underLiner.MovePage(false);
        }
    }
}
