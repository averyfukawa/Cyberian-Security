using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Player.Camera;
using UI.Translation;
using UI.Tutorial;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Raycasting.RotatingObjects
{
    public class RotatableObject : MonoBehaviour
    {
        /// <summary>
        /// All the rotations for the current object
        /// </summary>
        public List<RotationsSave> rotations = new List<RotationsSave>();
        /// <summary>
        /// The current index the rotation is at
        /// </summary>
        private int _currentIndex = 0;
        private bool _once = false;
        /// <summary>
        /// The buttons that rotate the object
        /// </summary>
        private Button[] _buttons;
        private Quaternion _originalRotation;
        private HoverOverObject _hoverObject;
        private LanguageScript.Languages _currentLanguage;
        

        [SerializeField] private bool _includeOriginal = true;

        // Start is called before the first frame update
        void Start()
        {
            _hoverObject = gameObject.GetComponent<HoverOverObject>();
            FolderMenu.setLanguageEvent += SetLanguage;
            foreach (var rotation in rotations)
            {
                rotation.SetFirst(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_hoverObject.GetPlaying())
            {
                if (!_once && !CameraMover.Instance.isMoving)
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

        /// <summary>
        /// The method to increase the current index for the rotation
        /// </summary>
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

        /// <summary>
        /// The method to decrease the current index for the rotation
        /// </summary>
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

        private void SetLanguage()
        {
            var languageScript = FindObjectOfType<LanguageScript>();
            _currentLanguage = languageScript.currentLanguage;
        }
        /// <summary>
        /// Check the current rotation. If the current rotation isn't at the end/start of the array it returns false
        /// </summary>
        /// <returns></returns>
        private bool IndexCheck()
        {
            //transform.rotation = _originalRotation;
            if (_currentIndex == -1)
            {
                _currentIndex = (rotations.Count - 1);
                if (_includeOriginal)
                {
                    transform.LeanRotateX((_originalRotation.eulerAngles.x), 0.3f).setDirection(-1);
                    transform.LeanRotateY((_originalRotation.eulerAngles.y), 0.3f).setDirection(-1);
                }
                else
                {
                    RotateObject();
                }
                
                return true;
            }

            if (_currentIndex == rotations.Count)
            {
                _currentIndex = 0;
                if (_includeOriginal)
                {
                    transform.LeanRotateX((_originalRotation.eulerAngles.x), 0.3f).setDirection(-1);
                    transform.LeanRotateY((_originalRotation.eulerAngles.y), 0.3f).setDirection(-1);
                }else
                {
                    RotateObject();
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Plays the audio associated with the current Rotation
        /// </summary>
        /// <param name="rs"></param>
        private void PlayAudio(RotationsSave rs)
        {
            if (rs.GetAudio() != null)
            {
                if (!FindObjectOfType<TutorialManager>()._doTutorial)
                {
                    if (rs.GetFirst())
                    {
                        rs.SetFirst(false);
                        Debug.Log("Should play audio here");
                    }  
                }
            }
        }

        /// <summary>
        /// Inverses the flip buttons. if they are active they become inactive.
        /// </summary>
        private void FlipButtons()
        {
            foreach (var button in _buttons)
            {
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }
        }

        /// <summary>
        /// Rotate the object to the rotation at the current index.
        /// </summary>
        private void RotateObject(){
            Debug.Log("index: " + _currentIndex);
            RotationsSave currentSave = rotations[_currentIndex];
            
            transform.LeanRotateX((currentSave.GetPosX()+ (int)_originalRotation.eulerAngles.x), 0.3f).setDirection(1);
            transform.LeanRotateY((currentSave.GetPosY()+ (int)_originalRotation.eulerAngles.y), 0.3f).setDirection(1);

            SetText(currentSave);
            PlayAudio(currentSave);
        }

        private void SetText(RotationsSave rs)
        {
            if (rs.GetFirst())
            {
                rs.SetFirst(false);
                StartCoroutine(MonologueAndWaitAdvance(FindObjectOfType<MonologueVisualizer>().VisualizeTextNonTutorial(rs.GetText(_currentLanguage))));
            }
        }
        
        private IEnumerator MonologueAndWaitAdvance(float waitingTime)
        {
            yield return new WaitForSeconds(waitingTime*.9f);
        }
        #endregion
        
    }

    #if UNITY_EDITOR
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
            _originalObject = _rotatableObject.transform.rotation;
            //_originalEditor = SceneView.lastActiveSceneView.pivot;
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
                    SaveCurrentRotation();
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
            if (_originalObject != null && _rotatableObject != null && _editorMode)
            {
                _rotatableObject.gameObject.transform.rotation = _originalObject;
            }

            _editorMode = false;
        }
        #endregion
        
        #region Methods

        /// <summary>
        /// Will enter editor mode allowing you to rotate the object without permanently putting object in that position
        /// </summary>
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
                //SceneView.lastActiveSceneView.pivot = _originalEditor;
                SceneView.lastActiveSceneView.Repaint();
            }
        }

        /// <summary>
        /// Save the rotation the editorMode is currently on
        /// </summary>
        /// <exception cref="WarningException"></exception>
        private void SaveCurrentRotation()
        {
            if (_editorMode)
            {
                Undo.RecordObject(_rotatableObject.gameObject, "saveRotation");
                Vector3 currentRotation = _rotatableObject.gameObject.transform.rotation.eulerAngles;
                _rotatableObject.rotations.Add(new RotationsSave((int) currentRotation.x,
                    (int) currentRotation.y));
                PrefabUtility.RecordPrefabInstancePropertyModifications(_rotatableObject.gameObject); // TODO make this stop misbehaving on prefabs, it should be called correctly here but somehow is not
            }
            else
            {
                throw new WarningException("Enter editor mode first");
            }
        }

        #endregion
    }
    #endif
}