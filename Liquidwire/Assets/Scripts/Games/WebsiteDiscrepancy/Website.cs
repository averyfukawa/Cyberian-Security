﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Website : MonoBehaviour
{
    public Button nextPage;
    private Page[] _pages;
    private Selectable[] _selectables;
    private int _currentPage = 0;
    // Start is called before the first frame update
    void Start()
    {
        _pages = gameObject.GetComponentsInChildren<Page>();
        _selectables = GetComponentsInChildren<Selectable>();
        for (int i = 0; i < _pages.Length; i++)
        {
            if (i != 0)
            {
                _pages[i].changeActive();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePage()
    {
        _pages[_currentPage].changeActive();
        
        if ((_currentPage+1) == _pages.Length)
        {
            _currentPage = 0;
        }
        else
        {
            _currentPage++;
        }
        _pages[_currentPage].changeActive();
    }
    

    public void CheckAllAnswers()
    {
        List<Selectable> wrongAnswers = new List<Selectable>();
        foreach (var selectable in _selectables)
        {
            Debug.Log(selectable.CheckAnswer());
            if (!selectable.CheckAnswer())
            {
                wrongAnswers.Add(selectable);
            }
        }

        if (wrongAnswers.Count == 0)
        {
            Debug.Log("Correct!");
        }else
        {
            Debug.Log("Incorrect!");
        }
    }
}