using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextComparison
{
    [Serializable]
    public class Discrepancy
    {
        public string word;
        
        public List<DcDifficultyStringDictionary> discrepancyDictionary;
        
        public Discrepancy(string word, List<DcDifficultyStringDictionary> discrepancyDictionary)
        {
            this.word = word;
            this.discrepancyDictionary = discrepancyDictionary;
        }
        /// <summary>
        /// Get the original word
        /// </summary>
        /// <returns></returns>
        public string GetWord()
        {
            return word;
        }
    }
}