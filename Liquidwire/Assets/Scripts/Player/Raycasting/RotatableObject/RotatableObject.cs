using System.Collections.Generic;
using UnityEngine;

namespace Player.Raycasting
{
    public class RotatableObject : MonoBehaviour
    {
        public List<RotationsSave> rotations = new List<RotationsSave>();
        private int _currentIndex = 0;
        private bool _once = false;
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
                    _originalRotation = gameObject.transform.rotation; 
                    _once = true;
                }
                if (Input.GetButtonDown("Debug Next"))
                {
                    if (!IndexCheck())
                    {
                        RotationsSave currentSave = rotations[_currentIndex];
                        transform.Rotate(currentSave.posX, currentSave.posY, 0);
                        _currentIndex++;
                    }
                } else if (Input.GetButtonDown("Debug Previous"))
                {
                    if (!IndexCheck())
                    {
                        RotationsSave currentSave = rotations[_currentIndex];
                        transform.Rotate(currentSave.posX, currentSave.posY, 0);
                        _currentIndex--;
                    } 
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
