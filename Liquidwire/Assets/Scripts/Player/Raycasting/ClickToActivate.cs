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
    public float maxDistance;
    public bool isExit;
    private bool _reconsider = false;
    [SerializeField] private List<CTATranslationDict> _translations;
    private GameObject _textField;
    private string _currentTranslationInspect;
    private string _currentTranslationHover;
    private GameObject _player;

    private GameObject _cameraObject;

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
    }

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
        _textField.SetActive(false);
    }

    private void ShowText(string textToShow)
    {
        if (!FindObjectOfType<TutorialManager>()._doTutorial)
        {
            StartCoroutine(MonologueAndWaitAdvance(FindObjectOfType<MonologueVisualizer>().VisualizeText(textToShow)));
        }
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

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}