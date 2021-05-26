using System.Collections;
using UI;
using UnityEngine;

namespace Player.Camera
{
    public class CameraMover : MonoBehaviour
    {
        /// <summary>
        /// Current instance of CameraMover
        /// </summary>
        public static CameraMover Instance;
        private MouseCamera _mouseCam;
        private UnityEngine.Camera _viewCamera;
        [SerializeField] private Transform _defaultCameraPos;
        /// <summary>
        /// A list with all the existing waypoints.
        /// </summary>
        [SerializeField] private Transform[] _targetPositions;
        /// <summary>
        /// Stores the instance of the player.
        /// </summary>
        private GameObject _player;
        /// <summary>
        /// If it is already moving
        /// </summary>
        public bool isMoving;
        private SFX _soundFolder;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("GameController");
            _viewCamera = UnityEngine.Camera.main;
            _mouseCam = FindObjectOfType<MouseCamera>();
            if (Instance == null)
            {
                Instance = this;
            }

            _soundFolder = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFX>();
        }

        #region Move camera

        /// <summary>
        /// move the camera to a pre-established waypoint and break mouse control over camera rotation
        /// </summary>
        /// <param name="positionIndex"></param>
        /// <param name="executionTime"></param>
        public void MoveCameraToPosition(int positionIndex, float executionTime)
        {
            StartCoroutine(ReAllowMovement(executionTime));
            _viewCamera.transform.LeanMove(_targetPositions[positionIndex].position, executionTime);
            _viewCamera.transform.LeanRotate(_targetPositions[positionIndex].rotation.eulerAngles, executionTime);
            _mouseCam.SetCursorNone();
        }

        /// <summary>
        /// return to original rotation and position
        /// </summary>
        /// <param name="executionTime"></param>
        public void ReturnCameraToDefault(float executionTime)
        {
            StartCoroutine(ReAllowMovement(executionTime));
            _viewCamera.transform.LeanMove(_defaultCameraPos.position, executionTime);
            _viewCamera.transform.LeanRotate(_defaultCameraPos.rotation.eulerAngles, executionTime);
            StartCoroutine(ReactivateCursorControl(executionTime));
        }

        #endregion

        #region Move object

        /// <summary>
        /// Move object to the position provided. This is used for picking it up and putting it down.
        /// </summary>
        /// <param name="positionIndex"></param>
        /// <param name="executionTime"></param>
        /// <param name="movingObject"></param>
        /// <param name="offsetAmount"></param>
        /// <param name="flip"></param>
        /// <param name="inspection"></param>
        public void MoveObjectToPosition(int positionIndex, float executionTime, GameObject movingObject,
            float offsetAmount,
            bool flip, bool inspection)
        {
            StartCoroutine(ReAllowMovement(executionTime, movingObject));
            movingObject.transform.LeanMove(
                _targetPositions[positionIndex].position + _targetPositions[positionIndex].forward * offsetAmount,
                executionTime);
            if (!inspection)
            {
                if (!flip)
                {
                    movingObject.transform.LeanRotate(_targetPositions[positionIndex].rotation.eulerAngles, executionTime);
                }
                else
                {
                    movingObject.transform.LeanRotateY(_targetPositions[positionIndex].rotation.eulerAngles.y + 180,
                        executionTime);
                }
            }

            _mouseCam.SetCursorNone();
        }
    
        /// <summary>
        /// Return the Object to the original position provided
        /// </summary>
        /// <param name="returnPosition"></param>
        /// <param name="returnRotation"></param>
        /// <param name="executionTime"></param>
        /// <param name="movingObject"></param>
        public void ReturnObjectToPosition(Vector3 returnPosition, Quaternion returnRotation, float executionTime,
            GameObject movingObject)
        {
            StartCoroutine(ReAllowMovement(executionTime));
            movingObject.transform.LeanMove(returnPosition, executionTime);
            movingObject.transform.LeanRotate(returnRotation.eulerAngles, executionTime);
            StartCoroutine(ReactivateCursorControl(executionTime));
            if (movingObject.TryGetComponent(out HelpFolder folder))
            {
                folder.ToggleOpen();

                _soundFolder.SoundFolderDown();
            }

            if (movingObject.TryGetComponent(out PrintPage page))
            {
                page.ToggleButton();
            }
        }

        #endregion

        #region unlocking player movement

        /// <summary>
        /// Reestablish the connection to the cursor control at the end to avoid snapping
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        private IEnumerator ReactivateCursorControl(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            ReactivateCursor();
        }

        /// <summary>
        /// Reactivate the cursor and the movement.
        /// </summary>
        public void ReactivateCursor()
        {
            _mouseCam.SetCursorLocked();
            _player.GetComponent<Movement>().ChangeLock();
        }

        /// <summary>
        /// Player can't interact with objects until after the provided time has passed.
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        private IEnumerator ReAllowMovement(float waitTime)
        {
            isMoving = true;
            yield return new WaitForSeconds(waitTime);
            isMoving = false;
        }

        /// <summary>
        /// Player can't interact with objects until after the provided time has passed.
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="movingObject"></param>
        /// <returns></returns>
        private IEnumerator ReAllowMovement(float waitTime, GameObject movingObject)
        {
            isMoving = true;
            yield return new WaitForSeconds(waitTime);
            if (movingObject.TryGetComponent(out HelpFolder folder))
            {
                folder.ToggleOpen();
                yield return new WaitForSeconds(folder._openingSpeed);
            }

            if (movingObject.TryGetComponent(out PrintPage page))
            {
                page.ToggleButton();
            }

            isMoving = false;
        }

        #endregion
    
    }
}