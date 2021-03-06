﻿using System.Collections;
using System.Collections.Generic;
using Enum;
using Player.Camera;
using Player.Save_scripts.Save_and_Load_scripts;
using TMPro;
using UI.Translation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Raycasting
{
    public class HoverOverObject : MonoBehaviour
    {
        /// <summary>
        /// The maximum distance where you can interact with the object
        /// </summary>
        [SerializeField] private float maxDistance;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private List<TranslationObject> translationList;

        /// <summary>
        /// Textfield that shows the "Use" text.
        /// </summary>
        private static GameObject _textField;

        private static GameObject _player;
        private bool _isPlaying = false;

        /// <summary>
        /// If the current object is a object you can pickup
        /// </summary>
        [SerializeField] private bool _isPickup = true;

        [SerializeField] private bool _isHelpNotes;

        /// <summary>
        /// this value is used to adjust the distance of a given object to be closer, or further
        /// </summary>
        [Range(-1f, .3f)] [SerializeField] private float _distanceAdjustment;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private bool _isActive = true;
        private GameObject _cameraObject;

        /// <summary>
        /// If it needs to be flipped
        /// </summary>
        public bool flipIt = false;

        /// <summary>
        /// If it needs to be rotated or not.
        /// </summary>
        [FormerlySerializedAs("isInspection")] public bool noRotateDuringPickUp = false;

        /// <summary>
        /// If the object only needs a hover text.
        /// </summary>
        public bool onlyHover;
        
        
        private LanguageScript.Languages _currentLanguage;
        private TranslationObject _currentTranslation;
        private TextMeshProUGUI _tmp;
        private bool _once = true;

        public virtual void Start()
        {
            if (_textField == null)
            {
                _textField = GameObject.FindGameObjectWithTag("HoverText");
                _player = GameObject.FindGameObjectWithTag("GameController");
            }
            FolderMenu.setLanguageEvent += SetLanguage;
            _cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            


            SetOriginPoints();
        }

        private void SetLanguage()
        {
            var _languageScript = FindObjectOfType<LanguageScript>();
            translationList = _textField.GetComponent<TranslationScript>()._translation;
            foreach (var language in translationList)
            {
                if (language.language== _languageScript.currentLanguage)
                {
                    _currentTranslation = language;
                    _tmp = _textField.GetComponent<TextMeshProUGUI>();
                }
            }
        }

        private void SetText()
        {
            SetLanguage();
            _tmp.text = _currentTranslation.translation;
        }
        #region Mouse functions

        public virtual void OnMouseOver()
        {
            float theDistance = Vector3.Distance(_cameraObject.transform.position, transform.position);
            if (!CameraMover.Instance.isMoving && _isActive)
            {
                // move into the screen view mode
                if (theDistance < maxDistance && !_isPlaying && !PlayerData.Instance.isInViewMode)
                {
                    if (_once)
                    {
                        SetText();
                        _once = false;
                    }
                    _textField.SetActive(true);
                    if (Input.GetButtonDown("Action"))
                    {
                        _textField.SetActive(false);
                        _player = GameObject.FindGameObjectWithTag("GameController");
                        if (!onlyHover)
                        {
                            if (!_isPickup)
                            {
                                CameraMover.Instance.MoveCameraToPosition((int) PositionIndexes.InFrontOfMonitor, 1.5f);
                                StartCoroutine(
                                    SetupVCAfterWait(
                                        1.5f)); // sets up the virtual canvas which is a necessity due to a b-ug with TMP
                                if (TutorialManager.Instance._doTutorial && TutorialManager.Instance.currentState ==
                                    TutorialManager.TutorialState.EmailOne)
                                {
                                    TutorialManager.Instance.AdvanceTutorial();
                                }
                            }
                            else
                            {
                                CameraMover.Instance.MoveObjectToPosition((int) PositionIndexes.InFrontOfCamera,
                                    1f, gameObject, _distanceAdjustment, flipIt, noRotateDuringPickUp);
                            }

                            _player.GetComponent<Movement>().ChangeLock();
                            _isPlaying = true;
                            PlayerData.Instance.isInViewMode = true;
                        }
                    }
                }
                else if (theDistance > maxDistance && _textField.activeSelf)
                {
                    _once = true;
                    _textField.SetActive(false);
                }
                // move out of the screen view mode
                else if (_isPlaying)
                {
                    if (Input.GetButtonDown("Cancel") && !Input.GetButtonDown("Action"))
                    {
                        _player = GameObject.FindGameObjectWithTag("GameController");

                        if (!_isPickup)
                        {
                            ReturnFromScreen();
                        }
                        else
                        {
                            if (TryGetComponent(out HelpFolder folder))
                            {
                                if (folder.CheckFolderMotion())
                                {
                                    return;
                                }
                            }

                            ReturnObject();
                        }
                    }
                }
            }
        }

        void OnMouseExit()
        {
            if (_textField.activeSelf)
            {
                _once = true;
                _textField.SetActive(false);
            }
        }

        #endregion

        #region Exit perspective

        /// <summary>
        /// Return the object to the original position.
        /// </summary>
        public void ReturnObject()
        {
            CameraMover.Instance.ReturnObjectToPosition(_originalPosition, _originalRotation,
                1f, gameObject);
            PlayerData.Instance.isInViewMode = false;
            _textField.SetActive(true);
            _isPlaying = false;
        }

        /// <summary>
        /// Return the camera to the original camera position.
        /// </summary>
        public void ReturnFromScreen()
        {
            CameraMover.Instance.ReturnCameraToDefault(1.5f);
            GetComponent<VirtualScreenSpaceCanvaser>()
                .ToggleCanvas(); // sets up the virtual canvas which is a necessity due to a b-ug with TMP
            StopCoroutine("SetupVCAfterWait");
            PlayerData.Instance.isInViewMode = false;
            _textField.SetActive(true);
            _isPlaying = false;
        }

        /// <summary>
        /// Forces the player out of the inspection state.
        /// </summary>
        public void ForceQuitInspect()
        {
            _textField.SetActive(true);
            _isPlaying = false;
            CameraMover.Instance.ReactivateCursor();
            PlayerData.Instance.isInViewMode = false;
        }

        #endregion

        /// <summary>
        /// Activate the VirtualScreen after the provided time has eclipsed
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        IEnumerator SetupVCAfterWait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            GetComponent<VirtualScreenSpaceCanvaser>().ToggleCanvas();
        }

        /// <summary>
        /// Set the original position and rotation
        /// </summary>
        public void SetOriginPoints()
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        public bool GetPlaying()
        {
            return _isPlaying;
        }

        public void ToggleActive()
        {
            _isActive = !_isActive;
        }
    }
}