﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Player.Save_scripts.Save_and_Load_scripts;
using UnityEngine;

public class FolderMenu : MonoBehaviour
{
    private PlayerData pd;
    private Movement move;
    private MouseCamera mc;
    [SerializeField] private Transform[] _folders = new Transform[6];
    private Vector3[] _folderPositions = new Vector3[6];
    private int _selectedfolder = -1;
    [SerializeField] private Camera _cam;
    private LayerMask _raycastMask;
    private Queue<int> _sequence = new Queue<int>();
    private bool _sequenceReady = true;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _folders.Length; i++)
        {
            _folderPositions[i] = _folders[i].position;
        }
        _raycastMask = LayerMask.GetMask("MenuFolders");
        pd = FindObjectOfType<PlayerData>();
        move = pd.gameObject.GetComponent<Movement>();
        move.isLocked = true;
        mc = move.gameObject.GetComponentInChildren<MouseCamera>();
        mc.SetCursorNone();
    }

    // Update is called once per frame
    void Update()
    {
        if (_sequenceReady && _sequence.Any())
        {
            StartCoroutine(ExecuteSequence());
        }
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 5f, _raycastMask))
        {
            for (int i = 0; i < _folders.Length; i++)
            {
                if (hit.transform.parent == _folders[i])
                {
                    if (_selectedfolder != i)
                    {
                        if (!_sequence.Contains(-(_selectedfolder + 1)) && _selectedfolder != -1)
                        {
                            _sequence.Enqueue(-(_selectedfolder+1));
                            PrioritizeInSequence(-(_selectedfolder+1));
                        }

                        if (!_sequence.Contains(i + 1))
                        {
                            _sequence.Enqueue(i+1);
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            if (_selectedfolder != -1)
            {
                _sequence.Enqueue(-(_selectedfolder+1));
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            switch (_selectedfolder)
            {
                case 0:
                    QuitGame();
                    break;
                case 1:
                    StartGame();
                    break;
                case 2:
                    LoadPlayer();
                    break;
                case 3:
                    MenuAudio();
                    break;
                case 4:
                    ShowCredits();
                    break;
                case 5:
                    ShowReading();
                    break;
            }
        }
    }

    private void LoadPlayer()
    {
        pd.LoadPlayer();
        Debug.Log("Load Player");
        StartGame();
    }

    private void StartGame()
    {
        
        Debug.Log("start game");
    }

    private void MenuAudio()
    {
        Debug.Log("audio menu");
    }

    private void ShowCredits()
    {
        
        Debug.Log("show credits");
    }
    
    private void ShowReading()
    {
        
        Debug.Log("show reading");
    }

    private void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        
    }

    private IEnumerator ExecuteSequence()
    {
        _sequenceReady = false;
        while (_sequence.Any())
        {
            SanitizeSequence();
            int actionValue = _sequence.Dequeue();
            switch (actionValue)
            {
                case 1:
                    _selectedfolder = 0;
                    _folders[0].LeanMove(_folderPositions[0] + new Vector3(0, 0, .6f), .5f);
                    yield return new WaitForSeconds(.5f);
                    break;
                case -1:
                    _selectedfolder = -1;
                    _folders[Mathf.Abs(actionValue)-1].LeanMove(_folderPositions[Mathf.Abs(actionValue)-1], .5f);
                    yield return new WaitForSeconds(.5f);
                    break;
                case int n when actionValue < -1:
                    _selectedfolder = -1;
                    _folders[Mathf.Abs(actionValue)-1].LeanMove(_folderPositions[Mathf.Abs(actionValue)-1], .2f);
                    yield return new WaitForSeconds(.2f);
                    break;
                case int n when actionValue > 1:
                    _selectedfolder = actionValue-1;
                    _folders[Mathf.Abs(actionValue)-1].LeanMove(_folderPositions[Mathf.Abs(actionValue)-1] + new Vector3(0, .02f, 0), .2f);
                    yield return new WaitForSeconds(.2f);
                    break;
            }
        }
        
        _sequenceReady = true;
    }

    private void PrioritizeInSequence(int priorityTargetIndex)
    {
        SanitizeSequence();
        Queue<int> prioritized = new Queue<int>();
        prioritized.Enqueue(priorityTargetIndex);
        foreach (var val in _sequence)
        {
            if (val != priorityTargetIndex)
            {
                prioritized.Enqueue(val);
            }
        }
        _sequence = prioritized;
    }

    private void SanitizeSequence()
    {
        int[] scoreBoard = new int[_folders.Length+1];
        foreach (var tween in _sequence)
        {
            scoreBoard[Mathf.Abs(tween)] += tween;
        }
        Queue<int> sanitized = new Queue<int>();
        foreach (var tween in _sequence)
        {
            if (scoreBoard[Mathf.Abs(tween)] != 0)
            {
                sanitized.Enqueue(tween);
            }

            scoreBoard[Mathf.Abs(tween)] = 0;
        }
        _sequence.Enqueue(-(_selectedfolder+1));
        _sequence = sanitized;
    }
}
