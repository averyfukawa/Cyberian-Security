using System.Collections.Generic;
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

        public void SetButtons(Button[] buttons)
        {
            this._buttons = buttons;
        }

        // Update is called once per frame
        void Update()
        {
            if (_hoverObject.GetPlaying())
            {
                if (!_once && !CameraMover.instance._isMoving)
                { 
                    _originalRotation = gameObject.transform.rotation;
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

        private void FlipButtons()
        { ;
            foreach (var button in _buttons)
            {
                button.gameObject.SetActive(!button.gameObject.activeSelf);
            }
        }

        public bool GetActive()
        {
            return _hoverObject.GetPlaying();
        }
        public void ClickUp()
        {
            if (_hoverObject.GetPlaying())
            {
                if (!IndexCheck())
                {
                    RotationsSave currentSave = rotations[_currentIndex];
                    transform.Rotate(currentSave.posX, currentSave.posY, 0);
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
                    RotationsSave currentSave = rotations[_currentIndex];
                    transform.Rotate(currentSave.posX, currentSave.posY, 0);
                    _currentIndex--;
                } 
            }
        }
            
        public bool IndexCheck()
        {
            transform.rotation = _originalRotation;
            if ( _currentIndex == -1)
            {
                _currentIndex = (rotations.Count-1);
                return true;
            } 
            if (_currentIndex == rotations.Count)
            {
                _currentIndex = 0;
                return true;
            }

            return false;
        }
    }
}
