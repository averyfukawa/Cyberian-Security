using UnityEngine;

namespace Player.Camera
{
    public class MouseCamera : MonoBehaviour
    {
        [SerializeField] private GameObject crosshairUI;
        public float mouseSens = 300f;
        public Transform playerBody;
        private bool _locked;

        private float _xRotation = 0f;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            crosshairUI.SetActive(true);
            _locked = true;
        }
        
        // Update is called once per frame
        void Update()
        {
            // update camera rotation each frame based on player rotation and mouse position
            if (_locked)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

                _xRotation -= mouseY;
                _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
                playerBody.Rotate(Vector3.up * mouseX);
                transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f); 
            }
        }
        
        #region change cursor mode

        /// <summary>
        /// Lock the cursor so that the camera moves with the mouse movements
        /// </summary>
        public void SetCursorLocked()
        {
            Cursor.lockState = CursorLockMode.Locked;
            crosshairUI.SetActive(true);
            _locked = true;
            mouseSens = 300f;
        }

        /// <summary>
        /// Unlock the cursor
        /// </summary>
        public void SetCursorNone()
        {
            Cursor.lockState = CursorLockMode.None;
            crosshairUI.SetActive(false);
            _locked = false;
            mouseSens = 0f;
        }

        #endregion

        public bool GetLockedState()
        {
            return _locked;
        }
    }
}