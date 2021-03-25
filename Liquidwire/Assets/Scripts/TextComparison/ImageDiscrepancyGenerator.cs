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
        [SerializeField] private GameObject _imageChild;
        public List<Color> randomColors;
        private TextMeshProUGUI _child;
        private bool _changed;

        public void Start()
        {
            _original = gameObject.GetComponent<RectTransform>().sizeDelta;
            _originalSplint = _imageChild.GetComponent<RectTransform>().sizeDelta;
            SetColor("#f2c100", _imageChild);
            SetColor("#008f8b", gameObject);
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

        public void ChangeFontSize()
        {
            _child.fontSize = 50;
            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
            _changed = true;
        }

        public void ChangeFont()
        {
            int index = Random.Range(0, list.Count);
            _child.font = list[index];
            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
            _changed = true;
        }

        public void ChangeSize()
        {
            int size = Random.Range(70, 130);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            _imageChild.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            _changed = true;
        }

        public void ChangeColor()
        {
            int index = Random.Range(0, randomColors.Count);
            int index2 = Random.Range(0, randomColors.Count);
            while (index == index2)
            {
                index2 = Random.Range(0, randomColors.Count);
            }
            
            _imageChild.GetComponent<Image>().color = randomColors[index];
            gameObject.GetComponent<Image>().color = randomColors[index2];

            gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
        }

        public void ResetAll()
        {
            gameObject.GetComponent<ImageDiscrepancy>().isScam = false;
            gameObject.GetComponent<RectTransform>().sizeDelta = _original;
            _imageChild.GetComponent<RectTransform>().sizeDelta = _originalSplint;

            SetColor("#f2c100", _imageChild);
            SetColor("#008f8b", gameObject);
            
            _child.font = list[0];
            _child.fontSize = 36;
        }

        public void SetColor(string code, GameObject image)
        {
            Color color = new Color();
            if (ColorUtility.TryParseHtmlString(code, out color))
            {
                image.GetComponent<Image>().color = color;
            }
        }
    }
}