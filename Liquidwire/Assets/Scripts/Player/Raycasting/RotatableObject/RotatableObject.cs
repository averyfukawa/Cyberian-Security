using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Raycasting
{
    public class RotatableObject : MonoBehaviour
    {
        public List<RotationsSave> rotations = new List<RotationsSave>();
        private int _currentIndex = 0;
        private bool _once = false;
        private Button[] _buttons;
        private Quaternion _originalRotation;
        private HoverOverObject _hoverObject;

        // Start is called before the first frame update
        void Start()
        {
            _hoverObject = gameObject.GetComponent<HoverOverObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_hoverObject.GetPlaying())
            {
                if (!_once && !CameraMover.instance._isMoving)
                {
                    _originalRotation = gameObject.transform.localRotation;
                    FlipButtons();
                    _once = true;
                }
            }
            else
            {
                if (_once)
                {
                    _once = false;
                    _currentIndex = 0;
                    FlipButtons();
                }
            }
        }

        #region Getters & Setters

        public void SetButtons(Button[] buttons)
        {
            this._buttons = buttons;
        }

        public bool GetActive()
        {
            return _hoverObject.GetPlaying();
        }

        #endregion

        #region OnClick methods

        public void ClickUp()
        {
            if (_hoverObject.GetPlaying())
            {
                if (!IndexCheck())
                {
                    RotateObject();
                    _currentIndex++;
                }
            }
        }

        public void ClickDown()
        {
            if (_hoverObject.GetPlaying())
            {
                if (!IndexCheck())
                {
                    RotateObject();
                    _currentIndex--;
                }
            }
        }

        #endregion

        #region Methods

        public bool IndexCheck()
        {
            //transform.rotation = _originalRotation;
            if (_currentIndex == -1)
            {
                transform.LeanRotateX((_originalRotation.eulerAngles.x), 0.3f).setDirection(-1);
                transform.LeanRotateY((_originalRotation.eulerAngles.y), 0.3f).setDirection(-1);
                _currentIndex = (rotations.Count - 1);
                return true;
            }

            if (_currentIndex == rotations.Count)
            {
                transform.LeanRotateX((_originalRotation.eulerAngles.x), 0.3f).setDirection(-1);
                transform.LeanRotateY((_originalRotation.eulerAngles.y), 0.3f).setDirection(-1);
                _currentIndex = 0;
                return true;
            }

            return false;
        }

        private void PlayAudio(RotationsSave rs)
        {
            if (rs.GetFirst())
            {
                rs.SetFirst(false);
                Debug.Log("Should play audio here");
            }
        }

        private void FlipButtons()
        {
            foreach (var button in _buttons)
            {
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }
        }

        public void RotateObject(){
            RotationsSave currentSave = rotations[_currentIndex];
            
            transform.LeanRotateX((currentSave.GetPosX()+ (int)_originalRotation.eulerAngles.x), 0.3f).setDirection(1);
            transform.LeanRotateY((currentSave.GetPosY()+ (int)_originalRotation.eulerAngles.y), 0.3f).setDirection(1);

            if (currentSave.GetAudio() != null)
            {
               PlayAudio(currentSave); 
            }
        }
        #endregion
        
    }

    [CustomEditor(typeof(RotatableObject))]
    public class RotatableEditor : UnityEditor.Editor
    {
        private RotatableObject _rotatableObject;
        private Vector3 _originalEditor;
        private Quaternion _originalObject;
        private bool _editorMode = false;
        private bool _oneTime = true;
        private int _delete;
        
        #region Inspector methods

        private void OnEnable()
        {
            _rotatableObject = target as RotatableObject;
            _originalEditor = SceneView.lastActiveSceneView.pivot;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (GUILayout.Button("Enter editor mode"))
                {
                    EnteringEditorMode();
                }

                if (GUILayout.Button("Save current rotation"))
                {
                    SaveCurrentPos();
                }
            }
            else
            {
                if (_oneTime)
                {
                    OnDestroy();
                    _oneTime = false;
                }
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete rotation at index: "))
            {
                _rotatableObject.rotations.RemoveAt(_delete);
            }

            _delete = EditorGUILayout.IntField(_delete);
            GUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            if (_originalObject != null && _rotatableObject != null)
            {
                _rotatableObject.gameObject.transform.rotation = _originalObject;
            }

            _editorMode = false;
        }
        #endregion
        
        #region Methods

        private void EnteringEditorMode()
        {
            _oneTime = true;
            if (!_editorMode)
            {
                SceneView.lastActiveSceneView.pivot = _rotatableObject.transform.position;
                SceneView.lastActiveSceneView.rotation = _rotatableObject.transform.rotation;
                SceneView.lastActiveSceneView.Repaint();
                _editorMode = true;
                _originalObject = _rotatableObject.transform.rotation;
            }
            else
            {
                _editorMode = false;
                SceneView.lastActiveSceneView.pivot = _originalEditor;
                SceneView.lastActiveSceneView.Repaint();
            }
        }

        private void SaveCurrentPos()
        {
            if (_editorMode)
            {
                Undo.RecordObject(_rotatableObject.gameObject, "saveRotation");
                Vector3 currentRotation = _rotatableObject.gameObject.transform.rotation.eulerAngles;
                _rotatableObject.rotations.Add(new RotationsSave((int) currentRotation.x,
                    (int) currentRotation.y));
                PrefabUtility.RecordPrefabInstancePropertyModifications(_rotatableObject.gameObject);
            }
            else
            {
                throw new WarningException("Enter editor mode first");
            }
        }

        #endregion
    }
}