using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TextComparison
{
    public class ImageDiscrepancyGenerator : MonoBehaviour
    {
        private string _fontPath;

        // reference to fontAssest
        public List<TMP_FontAsset> list;


        private void Start()
        {
            GenerateDiscrapency();
        }

        public void GenerateDiscrapency()
        {
            int difficulty = Random.Range(0, 100);
            TextMeshProUGUI child = gameObject.GetComponentInChildren<TextMeshProUGUI>();


            if (difficulty > 50)
            {
                int index = Random.Range(0, list.Count);
                child.font = list[index];
            }

            if (difficulty > 25)
            {
                child.fontSize = 50;
            }
        }
    }
}