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
        private Vector2 _original;
        private Vector2 _originalSplint;
        private Color _originalPieceColor;
        private Color _originalColor;
        [Range(0, 1)]
        [SerializeField] private float minDifference;
        [Range(0, 1)]
        [SerializeField] private float maxDifference;
        
        [SerializeField] private GameObject _imageChild;
        private TextMeshProUGUI _child;
        private bool _changed;

        public void Start()
        {
            _original = gameObject.GetComponent<RectTransform>().sizeDelta;
            _originalSplint = _imageChild.GetComponent<RectTransform>().sizeDelta;
            
            ColorUtility.TryParseHtmlString("#f2c100", out _originalPieceColor);
            ColorUtility.TryParseHtmlString("#008f8b", out _originalColor);
            
            _imageChild.GetComponent<Image>().color = _originalPieceColor;
            gameObject.GetComponent<Image>().color = _originalColor;
        }

        public void GenerateDiscrapency(int difficulty)
        {
            int rng = Random.Range(0, 100);
            _child = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _changed = false;
            if (rng > (difficulty * 4))
            {
                ChangeFontSize();
            }
            
            if (rng > (difficulty * 7))
            {
                ChangeFont();
            }
            
            if (rng > (difficulty * 9))
            {
                ChangeSize();
            }
            
            if (rng > (difficulty * 2))
            {
                ChangeColor();
            }
            
            if (!_changed)
            {
                ResetAll();
            }
        }

        private void ChangeFontSize()
        {
            _child.fontSize = 50;
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

        private void ChangeSize()
        {
            int size = Random.Range(70, 130);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            _imageChild.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            _changed = true;
        }

        private void ChangeColor()
        {
            float index = Random.Range(minDifference, maxDifference);

            _imageChild.GetComponent<Image>().color = IncrementColor(_originalPieceColor, index);
            gameObject.GetComponent<Image>().color = IncrementColor(_originalColor, index);
            
            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
        }

        private static Color IncrementColor(Color color, float index)
        {
            color.b -= index;
            color.g -= index;
            color.r -= index;
            return color;
        }

        private void ResetAll()
        {
            gameObject.GetComponent<ImageDiscrepancy>().isScam = false;
            gameObject.GetComponent<RectTransform>().sizeDelta = _original;
            _imageChild.GetComponent<RectTransform>().sizeDelta = _originalSplint;

            _imageChild.GetComponent<Image>().color = _originalPieceColor;
            gameObject.GetComponent<Image>().color = _originalColor;
            
            _child.font = list[0];
            _child.fontSize = 36;
        }
    }
}