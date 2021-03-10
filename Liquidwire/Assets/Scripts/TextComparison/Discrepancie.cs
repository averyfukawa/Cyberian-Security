using System;
using UnityEngine;

namespace TextComparison
{
    [Serializable]
    public class Discrepancie
    {
        public string word;
        public string[] discrepancie;


        public Discrepancie(string word, string[] discrepancie)
        {
            this.word = word;
            this.discrepancie = discrepancie;
        }
        public string GetWord()
        {
            return word;
        }
    }
}