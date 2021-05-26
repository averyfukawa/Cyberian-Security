using System.Collections;
using System.Collections.Generic;
using Player;
using Player.Save_scripts.Save_and_Load_scripts;
using UnityEngine;
using UnityEngine.UIElements;

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
    private LTSeq _seq;
    private Queue<int> _sequence = new Queue<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _folders.Length; i++)
        {
            _folderPositions[i] = _folders[i].position;
        }
        _raycastMask = LayerMask.GetMask("MenuFolders");
        /* pd = FindObjectOfType<PlayerData>();
        move = pd.gameObject.GetComponent<Movement>();
        move.isLocked = true;
        mc = move.gameObject.GetComponentInChildren<MouseCamera>();
        mc.SetCursorNone(); */
    }

    // Update is called once per frame
    void Update()
    {
        if (_seq == null && _sequence.Count > 0)
        {
            _seq = LeanTween.sequence();
            AppendDequeue();
        }
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 5f, _raycastMask))
        {
            for (int i = 0; i < _folders.Length; i++)
            {
                if (hit.transform.parent == _folders[i])
                {
                    if (_selectedfolder != i)
                    {
                        if (_selectedfolder > -1)
                        {
                            _sequence.Enqueue(-(_selectedfolder+1));
                        }
                        _selectedfolder = i;
                        _sequence.Enqueue(_selectedfolder+1);
                    }
                    break;
                }
            }
        }
        else
        {
            if (_selectedfolder > -1)
            {
                _sequence.Enqueue(-(_selectedfolder+1));
            }
            _selectedfolder = -1;
        }

        SanitizeSequence();
        
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
        // pd.LoadPlayer();
        Debug.Log("Load Player");
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

    private void AppendDequeue()
    {
        if (_sequence.Count == 0)
        {
            return;
        }
        int actionValue = _sequence.Dequeue();
        switch (actionValue)
        {
            case 1:
                _seq.append(_folders[0].LeanMove(_folderPositions[0] + new Vector3(0, 0, .2f), .2f));
                break;
            case int n when actionValue < 0:
                _seq.append(_folders[Mathf.Abs(actionValue)-1].LeanMove(_folderPositions[Mathf.Abs(actionValue)-1], .2f));
                break;
            case int n when actionValue > 1:
                _seq.append(_folders[Mathf.Abs(actionValue)-1].LeanMove(_folderPositions[Mathf.Abs(actionValue)-1] + new Vector3(0, .02f, 0), .2f));
                break;
        }
        _seq.append(AppendDequeue);
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

        _sequence = sanitized;
    }
}
