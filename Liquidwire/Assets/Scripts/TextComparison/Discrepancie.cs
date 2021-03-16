using System;
using System.Collections.Generic;
using UnityEngine;

namespace TextComparison
{
    [Serializable]
    public class Discrepancie
    {
        public string word;
        // public string[] discrepancieDictionary;
        public List<DcDifficultyStringDictionary> discrepancieDictionary;
        
        public Discrepancie(string word, List<DcDifficultyStringDictionary> discrepancieDictionary)
        {
            this.word = word;
            this.discrepancieDictionary = discrepancieDictionary;
        }
        public string GetWord()
        {
            return word;
        }
    }
}