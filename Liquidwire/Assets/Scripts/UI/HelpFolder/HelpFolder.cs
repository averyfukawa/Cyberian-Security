﻿using System;
using System.Collections;
using System.Collections.Generic;
using Games.TextComparison.Selectable_scripts;
using UnityEngine;

public class HelpFolder : MonoBehaviour
{
    [SerializeField] private Transform _topFlap;
    [SerializeField] private HelpPageViewer _helpViewer;
    [SerializeField] private CaseFolder _caseFolder;
    /// <summary>
    /// Speed at which the help folder is opened
    /// </summary>
    public float _openingSpeed = 1;
    [SerializeField] private float _rotationAmount;
    private bool _isOpen;
    public GameObject highlight;

    private void Start()
    {
        if (_helpViewer != null && _caseFolder != null)
        {
            Debug.Log("Folder setup incorrect, please un-assign either of the two folder page objects");
        }
        highlight.SetActive(false);
    }
    /// <summary>
    /// Check if the folder is still in motion.
    /// </summary>
    /// <returns></returns>
    public bool CheckFolderMotion()
    {
        if (_helpViewer != null)
        {
            return _helpViewer.inMotion;
        }
        else
        {
            return _caseFolder.inMotion;
        }
    }
    /// <summary>
    /// Open or Close the help folder.
    /// </summary>
    public void ToggleOpen()
    {
        _isOpen = !_isOpen;

        if (TutorialManager.Instance._doTutorial && highlight.activeSelf)
        {
            highlight.SetActive(false);
            TutorialManager.Instance.AdvanceTutorial();
        }

        if (_isOpen)
        {
            _topFlap.LeanRotateAroundLocal(Vector3.right, _rotationAmount, _openingSpeed);
            if (_caseFolder != null && _caseFolder.pages.Count > 0)
            {
                foreach (var CT in _caseFolder.pages.Peek().GetComponentsInChildren<ClickableText>())
                {
                   CT.SetActive(); 
                }
            }
        }
        else
        {
            _topFlap.LeanRotateAroundLocal(Vector3.right, -_rotationAmount, _openingSpeed/2);
            if (_caseFolder != null && _caseFolder.pages.Count > 0)
            {
                _caseFolder.GetComponentInChildren<UnderlineRender>().DropLines();
            }

            if (TutorialManager.Instance._doTutorial &&
                TutorialManager.Instance.currentState == TutorialManager.TutorialState.HelpfolderEnd)
            {
                TutorialManager.Instance.AdvanceTutorial();
            }
        }

        if (_helpViewer != null)
        {
            _helpViewer.ToggleButtons(_isOpen);
            _helpViewer.GetComponent<HelpStickyManager>().ToggleInteractable();
        }
        else if (_caseFolder != null && _isOpen)
        {
            StartCoroutine(ToggleAfterDelay(_openingSpeed));
        }
        else if (_caseFolder != null && !_isOpen)
        {
            _caseFolder.ToggleButtons(_isOpen);
        }
    }

    private IEnumerator ToggleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _caseFolder.ToggleButtons(_isOpen);
    }
}
