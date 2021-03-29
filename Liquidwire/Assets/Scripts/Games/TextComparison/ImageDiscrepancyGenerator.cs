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
        [Range(0, 1)] [SerializeField] private float minDifference;
        [Range(0, 1)] [SerializeField] private float maxDifference;

        [SerializeField] private GameObject _imageChild;
        private TextMeshProUGUI _child;
        private bool _changed;
        [SerializeField] private bool _textChild = false;

        public void Start()
        {
            _original = gameObject.GetComponent<RectTransform>().sizeDelta;
            _originalSplint = _imageChild.GetComponent<RectTransform>().sizeDelta;

            ColorUtility.TryParseHtmlString("#f2c100", out _originalPieceColor);
            ColorUtility.TryParseHtmlString("#008f8b", out _originalColor);

            _imageChild.GetComponent<Image>().color = _originalPieceColor;
            gameObject.GetComponent<Image>().color = _originalColor;

            if (gameObject.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                _child = gameObject.GetComponentInChildren<TextMeshProUGUI>();
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
            int index = Random.Range(20, 40);
            _child.fontSize = index;
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
            _imageChild.GetComponent<Image>().color = IncrementColor(_originalPieceColor);
            gameObject.GetComponent<Image>().color = IncrementColor(_originalColor);

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
            gameObject.GetComponent<RectTransform>().sizeDelta = _original;
            _imageChild.GetComponent<RectTransform>().sizeDelta = _originalSplint;

            _imageChild.GetComponent<Image>().color = _originalPieceColor;
            gameObject.GetComponent<Image>().color = _originalColor;

            _child.font = list[0];
            _child.fontSize = 36;
        }
    }
}