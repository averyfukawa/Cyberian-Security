using UnityEngine;
using UnityEngine.UI;

namespace Player.Raycasting.RotatingObjects
{
    public class InspectingButtons : MonoBehaviour
    {
        private Button[] _childs;
        private RotatableObject[] _rotatables;
        // Start is called before the first frame update
        void Start()
        {
            _childs = gameObject.GetComponentsInChildren<Button>();
            _rotatables = FindObjectsOfType<RotatableObject>();
            foreach (var item in _rotatables)
            {
                item.SetButtons(_childs);
            }
        
            foreach (var child in _childs)
            {
                child.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Check if there are any object currently moving.
        /// </summary>
        /// <returns></returns>
        private RotatableObject Check()
        {
            foreach (var item in _rotatables)
            {
                if (item.GetActive())
                {
                    return item;
                }
            }

            return null;
        }

        #region Rotation onClick scripts
        
        /// <summary>
        /// The click method to increase the current index for the rotation
        /// </summary>
        public void InspectRight()
        {
            var current = Check();
            current.ClickUp();
        }
        
        /// <summary>
        /// The click method to decrease the current index for the rotation
        /// </summary>
        public void InspectLeft()
        {
            var current = Check();
            current.ClickDown();
        }

        #endregion
    }
}
