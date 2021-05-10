using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Games.TextComparison.Selectable_scripts
{
    public class ImageDiscrepancy : MonoBehaviour
    {
        /// <summary>
        /// If the gameObject is a discrepancy
        /// </summary>
        public bool isScam;
        
        /// <summary>
        /// If it is selected
        /// </summary>
        private bool _isSelected;
        
        /// <summary>
        /// The circle drawn around the image to indicate if it has been selected.
        /// </summary>
        [SerializeField] private Image selectionCircle;

        private SFX _soundCircle;
        private void Start()
        {
            selectionCircle.fillAmount = 0;
            selectionCircle.gameObject.SetActive(false);
        
            _soundCircle = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFX>();
        }
        /// <summary>
        /// Change the state of the image. if it is being selected it will draw a circle around the image.
        /// Otherwise it will delete the circle.
        /// </summary>
        public void ChangeSelected()
        {
            _isSelected = !_isSelected;
            if (_isSelected)
            {
                selectionCircle.gameObject.SetActive(true);
                StartCoroutine(AnimateCircling(.3f));
            }
            else
            {
                selectionCircle.fillAmount = 0;
                // TODO add shader for eraser here (probably using a mask running through a shadered texture ?)
                selectionCircle.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// This function will animate the drawing of the circle around an image, based on the time provided
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator AnimateCircling(float time)
        {
            float timeSpent = 0;
            _soundCircle.SoundPencilCircling();
            while (selectionCircle.fillAmount < 1)
            {
                timeSpent += Time.deltaTime;
         
                selectionCircle.fillAmount = timeSpent / time;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Check if it's selected and if it is a scam. it will cross-reference the 2 variables if they correspond it
        /// is correct
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            return _isSelected == isScam;
        }

        public void ResetSelected()
        {
            _isSelected = !_isSelected;
            selectionCircle.fillAmount = 0;
            // TODO add shader for eraser here (probably using a mask running through a shadered texture ?)
            selectionCircle.gameObject.SetActive(false);
        }


        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawCube(transform.position, GetComponent<RectTransform>().sizeDelta);
        // }
    }
}
