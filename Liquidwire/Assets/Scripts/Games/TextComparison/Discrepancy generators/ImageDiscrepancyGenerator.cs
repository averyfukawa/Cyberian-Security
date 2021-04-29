using System.Collections.Generic;
using Games.TextComparison.Selectable_scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Games.TextComparison.Discrepancy_generators
{
    public class ImageDiscrepancyGenerator : MonoBehaviour
    {
        private string _fontPath;

        private bool _once = false;
        // reference to fontAssets
        public List<TMP_FontAsset> list = new List<TMP_FontAsset>();
        private List<Color> _originalColors = new List<Color>();
        [Range(0, 1)] [SerializeField] private float minDifference;
        [Range(0, 1)] [SerializeField] private float maxDifference;

        [SerializeField] private GameObject[] imageChildren = new GameObject[0];
        private TextMeshProUGUI _child;
        private float _originalFontSize;
        private bool _changed;
        [SerializeField] private bool textChild = false;

        public void Start()
        {
;
            foreach (var img in imageChildren)
            {
                _originalColors.Add(img.GetComponent<Image>().color);
            }
            
            if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                _child = gameObject.GetComponentInChildren<TextMeshProUGUI>();
                _originalFontSize = _child.fontSize;
                textChild = true;
            }
        }
        
        /// <summary>
        /// Creates the necessary variables.
        /// </summary>
        public void StartUp()
        {
            if (!_once)
            {
               _originalColors = new List<Color>();
                foreach (var img in imageChildren) 
                { 
                    _originalColors.Add(img.GetComponent<Image>().color);
                }
                if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null) 
                { 
                    _child = gameObject.GetComponentInChildren<TextMeshProUGUI>(); _originalFontSize = _child.fontSize; textChild = true;
                } 
                _once = true;
            }
        }
        
        /// <summary>
        /// Generate a discrepancy based on the difficulty.
        /// </summary>
        /// <param name="difficulty"></param>
        public void GenerateDiscrepancy(int difficulty)
        {
            ResetAll();
            int rng = Random.Range(0, 100);

            _changed = false;
            if (rng > (difficulty * 7) && textChild)
            {
                ChangeFont();
            }
            else if (rng > (difficulty * 4) && textChild)
            {
                ChangeFontSize();
            }
            else if (rng > (difficulty * 2))
            {
                ChangeColor();
            }
            else
            {
                ResetAll();
            }
        }

        /// <summary>
        /// Change the size of the font of the text.
        /// </summary>
        private void ChangeFontSize()
        {
            float currentFontsize = _child.fontSize;
            _child.fontSize = Random.Range(currentFontsize*.8f, currentFontsize*1.2f);
            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
            _changed = true;
        }

        /// <summary>
        /// Change the font of the text next to the image
        /// </summary>
        private void ChangeFont()
        {
            int index = Random.Range(0, list.Count);
            _child.font = list[index];
            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
            _changed = true;
        }
        
        /// <summary>
        /// Change the colors of all the images.
        /// </summary>
        private void ChangeColor()
        {
            for (int i = 0; i < imageChildren.Length; i++)
            {
                imageChildren[i].GetComponent<Image>(). color = IncrementColor(_originalColors[i]);
            }

            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
        }
        
        /// <summary>
        /// Change the color of the image by a couple gradients
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Color IncrementColor(Color color)
        {
            float index = Random.Range(minDifference, maxDifference);
            color.b -= index;
            color.g -= index;
            color.r -= index;
            return color;
        }

        /// <summary>
        /// Reset the image back to the original color.
        /// </summary>
        private void ResetAll()
        {
            gameObject.GetComponent<ImageDiscrepancy>().isScam = false;
            for (int i = 0; i < imageChildren.Length; i++)
            {
                imageChildren[i].GetComponent<Image>().color = _originalColors[i];
            }
            _child.font = list[0];
            _child.fontSize = _originalFontSize;
        }
    }
}