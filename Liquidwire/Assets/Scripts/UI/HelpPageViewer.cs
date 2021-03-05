using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpPageViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField] private GameObject[] _pageButtons = new GameObject[2];

    private void Start()
    {
        _pageButtons[1].SetActive(false);
    }

    public void ChangePage(bool isForward)
    {
        if (isForward)
        {
            _pageText.pageToDisplay++;
            if (!_pageButtons[1].activeSelf)
            {
                _pageButtons[1].SetActive(true);
            }
        }
        else
        {
            _pageText.pageToDisplay--;
            if (_pageText.pageToDisplay < 2)
            {
                _pageButtons[1].SetActive(false);
            }
        }
    }
}
