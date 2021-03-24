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

        // reference to fontAssest
        public List<TMP_FontAsset> list;
        private Vector2 _original;

        public void Start()
        {
            _original = gameObject.GetComponent<RectTransform>().sizeDelta;
        }

        public void GenerateDiscrapency(int difficulty)
        {
            int rng = Random.Range(0, 100);
            TextMeshProUGUI child = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            bool changed = false;


            if (rng > (difficulty * 4))
            {
                child.fontSize = 50;
                gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
                changed = true;
            }

            if (rng > (difficulty * 7))
            {
                int index = Random.Range(0, list.Count);
                child.font = list[index];
                gameObject.GetComponent<ImageDiscrepancy>().isScam = true;
                changed = true;
            }

            if (rng > (difficulty * 9))
            {
                int size = Random.Range(70, 130);
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                changed = true;
            }

            if (rng > (difficulty * 2))
            {
                //TODO: Change the obnoxiously bright colors
                var temp = gameObject.GetComponent<RawImage>().color;
                temp.a = 1;
            }

            if (!changed)
            {
                gameObject.GetComponent<ImageDiscrepancy>().isScam = false;
                gameObject.GetComponent<RectTransform>().sizeDelta = _original;
                child.font = list[0];
                child.fontSize = 36;
            }
        }
    }
}