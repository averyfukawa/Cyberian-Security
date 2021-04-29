using Player.Camera;
using Player.Save_scripts.Save_and_Load_scripts;
using UnityEngine;

namespace Player.Save_scripts.Save_system_interaction
{
    public class SaveMenuButtons : MonoBehaviour
    {
        private PlayerData _pd;
        private Movement _move;
        private MouseCamera _mc;
        public void Start()
        {
            _pd = FindObjectOfType<PlayerData>();
            _move = _pd.gameObject.GetComponent<Movement>();
            _move.isLocked = true;
            _mc = _move.gameObject.GetComponentInChildren<MouseCamera>();
            _mc.SetCursorNone();
        }

        public void Update()
        {
            if (_mc.GetLockedState())
            {
                _mc.SetCursorNone();
            }
        }

        #region OnClick methods

        /// <summary>
        /// Click method to load the previous save of the game.
        /// </summary>
        public void LoadPlayer()
        {
            _mc.SetCursorLocked();
            _move.isLocked = false;
            _pd.LoadPlayer();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Click method to start the game normally
        /// </summary>
        public void StartGame()
        {
            gameObject.SetActive(false);
            _mc.SetCursorLocked();
            _move.isLocked = false;
        }

        #endregion
        
    }
}
