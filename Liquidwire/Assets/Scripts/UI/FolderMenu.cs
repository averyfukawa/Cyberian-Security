using System.Collections;
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
    private bool _allowAction;
    private Camera _gameplayCam;
    
    [SerializeField] RectTransform[] _audioMenuBackgrounds = new RectTransform[3];
    Vector2[] _audioMenuBackgroundTargetsPos = new Vector2[3];
    Quaternion[] _audioMenuBackgroundTargetsRot = new Quaternion[3];
    [SerializeField] private RectTransform audioMenuRoot;
    private bool _audioMenuOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _folders.Length; i++)
        {
            _folderPositions[i] = _folders[i].position;
        }
        for (int i = 0; i < _audioMenuBackgrounds.Length; i++)
        {
            _audioMenuBackgroundTargetsPos[i] = _audioMenuBackgrounds[i].anchoredPosition;
            _audioMenuBackgroundTargetsRot[i] = _audioMenuBackgrounds[i].rotation;
            _audioMenuBackgrounds[i].anchoredPosition = audioMenuRoot.anchoredPosition;
            _audioMenuBackgrounds[i].rotation = audioMenuRoot.rotation;
        }
        _raycastMask = LayerMask.GetMask("MenuFolders");
        _folders[0].Translate(new Vector3(0, 0, .7f));
        StartCoroutine(StartSlideOut(1f));
        pd = FindObjectOfType<PlayerData>();
        move = pd.gameObject.GetComponent<Movement>();
        move.isLocked = true;
        mc = move.gameObject.GetComponentInChildren<MouseCamera>();
        mc.SetCursorNone();
        StartCoroutine(WaitForCam());
    }

    private IEnumerator WaitForCam()
    {
        yield return new WaitForEndOfFrame();
        _gameplayCam = Camera.main;
        _gameplayCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_sequenceReady && _sequence.Any())
        {
            StartCoroutine(ExecuteSequence());
        }

        bool hitBlock = false;
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 5f, _raycastMask) && _allowAction)
        {
            if (hit.transform.CompareTag("MenuBlock"))
            {
                hitBlock = true;
            }
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
        
        if (Input.GetMouseButtonUp(0) && !hitBlock)
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
        StartCoroutine(DoCamTransition());
        _sequence.Enqueue(-(_selectedfolder+1));
        PrioritizeInSequence(-(_selectedfolder+1));
    }

    private void StartGame()
    {
        StartCoroutine(DoCamTransition());
        TutorialManager.Instance.DoTutorial();
        Debug.Log("start game");
        _sequence.Enqueue(-(_selectedfolder+1));
        PrioritizeInSequence(-(_selectedfolder+1));
    }

    private void MenuAudio()
    {
        for (var index = 0; index < _audioMenuBackgrounds.Length; index++)
        {
            StartCoroutine(SlideAudioMenuPiece(index, _audioMenuOpen));
        }

        Debug.Log("toggle audio menu");
    }

    private IEnumerator SlideAudioMenuPiece(int index, bool isSlidingIn)
    {
        float timer = 0;
        float timeInSec = .5f + .1f * index;
        Vector2 startPos = _audioMenuBackgrounds[index].anchoredPosition;
        Quaternion startRot = _audioMenuBackgrounds[index].rotation;
        if (isSlidingIn)
        {
            while (timer < timeInSec)
            {
                timer += Time.deltaTime;
                float scaleFactor = timer / timeInSec;
                _audioMenuBackgrounds[index].anchoredPosition = Vector2.Lerp(startPos, audioMenuRoot.anchoredPosition
                    , scaleFactor);
                _audioMenuBackgrounds[index].rotation = Quaternion.Slerp(startRot, audioMenuRoot.rotation, scaleFactor);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (timer < timeInSec)
            {
                timer += Time.deltaTime;
                float scaleFactor = timer / timeInSec;
                _audioMenuBackgrounds[index].anchoredPosition = Vector2.Lerp(startPos, _audioMenuBackgroundTargetsPos[index], scaleFactor);
                _audioMenuBackgrounds[index].rotation = Quaternion.Slerp(startRot, _audioMenuBackgroundTargetsRot[index], scaleFactor);
                yield return new WaitForEndOfFrame();
            } 
        }

        _audioMenuOpen = !isSlidingIn;
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
                    if (_audioMenuOpen)
                    {
                        MenuAudio();
                    }
                    _folders[Mathf.Abs(actionValue)-1].LeanMove(_folderPositions[Mathf.Abs(actionValue)-1], .2f);
                    yield return new WaitForSeconds(.2f);
                    break;
                case int n when actionValue > 1:
                    _selectedfolder = actionValue-1;
                    _folders[Mathf.Abs(actionValue)-1].LeanMove(_folderPositions[Mathf.Abs(actionValue)-1] + new Vector3(0, .04f, 0), .2f);
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

    private IEnumerator StartSlideOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        _sequence.Enqueue(-1);
        yield return new WaitForSeconds(1);
        _allowAction = true;
    }
    
    private IEnumerator DoCamTransition()
    {
        mc.SetCursorLocked();
        _allowAction = false;
        yield return new WaitForSeconds(.2f);
        _folders[0].LeanMove(_folderPositions[0] + new Vector3(0, 0, .7f), .5f);
        _cam.transform.LeanMove(_gameplayCam.transform.position, 1.5f);
        _cam.transform.LeanRotate(_gameplayCam.transform.rotation.eulerAngles, 1.5f);
        yield return new WaitForSeconds(1.5f);
        if (!TutorialManager.Instance._doTutorial)
        {
            move.isLocked = false;
        }
        _gameplayCam.enabled = true;
        _cam.gameObject.SetActive(false);
    }
}
