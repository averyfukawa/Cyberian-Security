using System;
using System.Collections.Generic;

namespace TextComparison
{
    [Serializable]
    public class DcDifficultyStringDictionary
    {
        public List<string> discrepancies;
        public List<int> difficulty;


        public DcDifficultyStringDictionary(List<string> discrepancies, List<int> difficulty)
        {
            this.discrepancies = discrepancies;
            this.difficulty = difficulty;
        }
    }
    
    
}