using System;
using System.Collections.Generic;

namespace TextComparison
{
    [Serializable]
    public class DcDifficultyStringDictionary
    {
        public string discrepancies;
        public int difficulty;

        public DcDifficultyStringDictionary(string discrepancies, int difficulty)
        {
            this.discrepancies = discrepancies;
            this.difficulty = difficulty;
        }
        
    }
    
    
}