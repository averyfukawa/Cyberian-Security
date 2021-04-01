using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TextComparison
{
    public class ImageDiscrepancyGenerator : MonoBehaviour
    {
        private string _fontPath;

        // reference to fontAssets
        public List<TMP_FontAsset> list;
        private List<Color> _originalColors = new List<Color>();
        [Range(0, 1)] [SerializeField] private float minDifference;
        [Range(0, 1)] [SerializeField] private float maxDifference;

        [SerializeField] private GameObject[] _imageChildren = new GameObject[2];
        private TextMeshProUGUI _child;
        private float _originalFontSize;
        private bool _changed;
        [SerializeField] private bool _textChild = false;

        public void Start()
        {
;
            foreach (var img in _imageChildren)
            {
                _originalColors.Add(img.GetComponent<Image>().color);
            }

            if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                _child = gameObject.GetComponentInChildren<TextMeshProUGUI>();
                _originalFontSize = _child.fontSize;
                _textChild = true;
            }
        }

        public void GenerateDiscrapency(int difficulty)
        {
            ResetAll();
            int rng = Random.Range(0, 100);

            _changed = false;
            if (rng > (difficulty * 7) && _textChild)
            {
                ChangeFont();
            }
            else if (rng > (difficulty * 4) && _textChild)
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

        private void ChangeFontSize()
        {
            float currentFontsize = _child.fontSize;
            _child.fontSize = Random.Range(currentFontsize*.8f, currentFontsize*1.2f);
            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
            _changed = true;
        }

        private void ChangeFont()
        {
            int index = Random.Range(0, list.Count);
            _child.font = list[index];
            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
            _changed = true;
        }

        private void ChangeColor()
        {
            for (int i = 0; i < _imageChildren.Length; i++)
            {
                _imageChildren[i].GetComponent<Image>(). color = IncrementColor(_originalColors[i]);
            }

            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
        }

        private Color IncrementColor(Color color)
        {
            float index = Random.Range(minDifference, maxDifference);
            color.b -= index;
            color.g -= index;
            color.r -= index;
            return color;
        }

        private void ResetAll()
        {
            gameObject.GetComponent<ImageDiscrepancy>().isScam = false;

            for (int i = 0; i < _imageChildren.Length; i++)
            {
                _imageChildren[i].GetComponent<Image>(). color = _originalColors[i];
            }

            _child.font = list[0];
            _child.fontSize = _originalFontSize;
        }
    }
}