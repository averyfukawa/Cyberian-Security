using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using Player.Camera;
using Player.Save_scripts.Save_and_Load_scripts;
using TMPro;
using UI.Translation;
using UI.Tutorial;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ClickToActivate : MonoBehaviour
{
    /// <summary>
    /// Maximum distance allowed to interact with the object
    /// </summary>
    public float maxDistance;
    
    /// <summary>
    /// Is the quit the game object
    /// </summary>
    public bool isExit;
    
    /// <summary>
    /// Used to confirm the action of quitting the game.
    /// </summary>
    private bool _reconsider = false;
    [SerializeField] private List<CTATranslationDict> _translations;
    private GameObject _textField;
    
    /// <summary>
    /// Text shown when player clicks on the object.
    /// </summary>
    private string _currentTranslationInspect;
    
    /// <summary>
    /// text shown when player hovers over the object
    /// </summary>
    private string _currentTranslationHover;
    private GameObject _player;

    private GameObject _cameraObject;

    #region Methods at the start
    
    // Start is called before the first frame update
    void Start()
    {
        if (_textField == null)
        {
            _textField = GameObject.FindGameObjectWithTag("HoverText");
            _player = GameObject.FindGameObjectWithTag("GameController");
        }

        FolderMenu.setLanguageEvent += SetLanguage;
        _cameraObject = UnityEngine.Camera.main.gameObject;
        StartCoroutine(WaitForHover());
    }
    private void SetLanguage()
    {
        var languageScript = FindObjectOfType<LanguageScript>();
        foreach (var item in _translations)
        {
            if (item.language == languageScript.currentLanguage)
            {
                _currentTranslationInspect = item.translationInspect;
                _currentTranslationHover = item.translationHover;
            }
        }
    }
    private IEnumerator WaitForHover()
    {
        yield return new WaitForEndOfFrame();
        _textField.SetActive(false);
    }

    #endregion

    #region Mouse methods

    private void OnMouseOver()
    {
        float theDistance = Vector3.Distance(_cameraObject.transform.position, transform.position);
        if (!CameraMover.Instance.isMoving)
        {
            if (theDistance < maxDistance && !PlayerData.Instance.isInViewMode)
            {
                SetHoverText();
                if (!_reconsider)
                {
                    if (Input.GetButtonDown("Action"))
                    {
                        ShowText(_currentTranslationInspect);
                        if (isExit)
                        {
                            _reconsider = true;
                        }
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Action"))
                    {
                        QuitGame();
                    }
                    else if (Input.GetButtonDown("Cancel"))
                    {
                        _reconsider = false;
                    }
                }
            }
            else if (theDistance > maxDistance && _textField.activeSelf)
            {
                _textField.SetActive(false);
            }
        }
    }

    void OnMouseExit()
    {
        if (_textField.activeSelf)
        {
            _textField.SetActive(false);
        }
        _reconsider = false;
    }

    #endregion

    #region Text methods

    private void ShowText(string textToShow)
    {
        StartCoroutine(MonologueAndWaitAdvance(FindObjectOfType<MonologueVisualizer>().VisualizeTextNonTutorial(textToShow)));
    }

    private void SetHoverText()
    {
        _textField.GetComponent<TextMeshProUGUI>().text = _currentTranslationHover;
        _textField.SetActive(true);
    }

    private IEnumerator MonologueAndWaitAdvance(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime * .9f);
    } 

    #endregion
    
    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}