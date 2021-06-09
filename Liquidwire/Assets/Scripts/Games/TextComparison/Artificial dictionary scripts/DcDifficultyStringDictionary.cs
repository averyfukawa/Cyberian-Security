using System;
using System.Collections.Generic;

namespace TextComparison
{
    [Serializable]
    // This is a artificial dictionary so that we can see it in the inspector as well
    public class DcDifficultyStringDictionary
    {
        /// <summary>
        /// The discrepancy word
        /// </summary>
        public string discrepancies;
        /// <summary>
        /// The difficulty of the discrepancy word.
        /// </summary>
        public int difficulty;

        public DcDifficultyStringDictionary(string discrepancies, int difficulty)
        {
            this.discrepancies = discrepancies;
            this.difficulty = difficulty;
        }
        
    }
    
    
}