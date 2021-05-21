using UnityEngine;

namespace Player.Camera
{
        /// </summary>
        /// If the mouse is locked or not
        /// <summary>

    public void SetCursorLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshairUI.SetActive(true);
        _locked = true;
        mouseSens = 300f;
    }

    public void SetCursorNone()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshairUI.SetActive(false);
        _locked = false;
        mouseSens = 0f;
    }

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