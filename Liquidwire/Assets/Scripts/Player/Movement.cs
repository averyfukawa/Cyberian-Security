using System.Collections;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float speed = 6.0F;
        public float gravity = 20.0F;
        private Vector3 _moveDirection = Vector3.zero;
        public bool isLocked = false;

        /// <summary>
        /// Check if the players has moved yet.
        /// </summary>
        private bool _hasMoved;
        private float _movementTutorialTimer;
        [SerializeField] private float movementTutorialDelay;
        [SerializeField] private GameObject _movementTutorialObject;

        private void Start()
        {
            _movementTutorialObject.SetActive(false);
        }

        void Update()
        {
            CharacterController controller = GetComponent<CharacterController>();
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= speed;
            _moveDirection.y -= gravity * Time.deltaTime;
            if (!isLocked)
            {
                if (TutorialManager.Instance._doTutorial &&
                    TutorialManager.Instance.currentState == TutorialManager.TutorialState.Standup)
                {
                    ChangeLock();
                    TutorialManager.Instance.AdvanceTutorial();
                    return;
                }
                _hasMoved = true;
                if (_movementTutorialObject.activeSelf)
                {
                    _hasMoved = true;
                    if (_movementTutorialObject.activeSelf)
                    {
                        StartCoroutine(FadeTutorialHelp(4f));
                    }
                }
                controller.Move(_moveDirection * Time.deltaTime);
            }

            if (!_hasMoved && ! isLocked)
            {
                _movementTutorialTimer += Time.deltaTime;
                if (_movementTutorialTimer >= movementTutorialDelay && !_movementTutorialObject.activeSelf)
                {
                    _movementTutorialObject.SetActive(true);
                }
            }
        }
        
        
        
        /// <summary>
        /// This will make the tutorial text disappear
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator FadeTutorialHelp(float time)
        {
            TextMeshProUGUI textMesh = _movementTutorialObject.GetComponent<TextMeshProUGUI>();
            float timeSpent = 0;
            Color newColour = textMesh.color;
            while (textMesh.color.a > 0)
            {
                timeSpent += Time.deltaTime;
         
                newColour.a = 1 - timeSpent / time;
                textMesh.color = newColour;
                yield return new WaitForEndOfFrame();
            }
            _movementTutorialObject.SetActive(false);
        }

        /// <summary>
        /// This will invert the movement lock
        /// </summary>
        public void ChangeLock()
        {
            isLocked = !isLocked;
        }
    
    }
}