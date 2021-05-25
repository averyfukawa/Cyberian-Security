using System.Collections;
using System.Collections.Generic;
using Games.WebsiteDiscrepancy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Website : MonoBehaviour
{
    public GameObject nextPage;
    public GameObject messagingPage;
    public GameObject finishButton;
    public GameObject messagingButton;
    /// <summary>
    /// List of all the pages in this website
    /// </summary>
    private Page[] _pages;
    /// <summary>
    /// list of all the selectables in this website
    /// </summary>
    private Selectable[] _selectables;
    private int _currentPage = 0;
    /// <summary>
    /// If the player is currently using the guided conversation system.
    /// </summary>
    private bool _messaging = false;
    // Start is called before the first frame update
    void Start()
    {
        _pages = gameObject.GetComponentsInChildren<Page>();
        _selectables = GetComponentsInChildren<Selectable>();
        for (int i = 0; i < _pages.Length; i++)
        {
            if (i != 0)
            {
                _pages[i].ChangeActive();
            }
        }
        messagingPage.SetActive(false);
    }
    
    /// <summary>
    /// This will set the next page as the current page
    /// </summary>
    public void ChangePage()
    {
        _pages[_currentPage].ChangeActive();
        
        if ((_currentPage+1) == _pages.Length)
        {
            _currentPage = 0;
        }
        else
        {
            _currentPage++;
        }
        _pages[_currentPage].ChangeActive();
    }

    /// <summary>
    /// Inverts all the current active components to hide them.
    /// </summary>
    public void ChangeMessaging()
    {
        _pages[_currentPage].ChangeActive();
        nextPage.SetActive(!nextPage.activeSelf);
        messagingButton.SetActive(!messagingButton.activeSelf);
        finishButton.SetActive(!finishButton.activeSelf);
        messagingPage.SetActive(!_messaging);
        _messaging = !_messaging;
    }
    
    /// <summary>
    /// Check the answers of all the pages
    /// </summary>
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