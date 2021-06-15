using System.Collections.Generic;

namespace Games.PointsCalculation
{
    public class CaseGrading
    {
        private readonly Dictionary<int, int> _difficultyCounters;
        public CaseGrading()
        {
            /***
             * Difficulty counter is a list where the Key is the difficulty(Left Integer) and the value is
             * the amount of errors allowed(Right Integer).
             */
            _difficultyCounters = new Dictionary<int, int>
            {
                {1, 2},
                {2, 2},
                {3, 2},
                {4, 3},
                {5, 3},
                {6, 3},
                {7, 4},
                {8, 4},
                {9, 4},
                {10, 5}
            };
        }

        #region Evaluation
        /// <summary>
        /// Will return a bool based on the difficulty and the amount of errors. The allowed amount of errors are
        /// stored in a list in the CaseGrading class.
        /// </summary>
        /// <param name="amountOfErrors"></param>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public bool Evaluation(int amountOfErrors, int difficulty)
        {
            _difficultyCounters.TryGetValue(difficulty ,out int maxCount);
            if (amountOfErrors > maxCount)
            {
                return false;
            }

            return true;
        }

        #endregion
    
        #region Points
        /// <summary>
        /// This will calculate the points based on the following:"((100/timeInSeconds)*difficulty)/amountOfErrors"
        /// if the user completes it without an error they will get a bonus 1 point, making them elligable for the full
        /// 10 marks.
        /// </summary>
        /// <param name="timeInSeconds"></param>
        /// <param name="amountOfErrors"></param>
        /// <param name="difficulty"></param>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        public float Points(float timeInSeconds, float amountOfErrors, float difficulty, bool evaluation)
        {
            float points = 0.0f;
            var bonus = 0;
            if (amountOfErrors == 0)
            {
                bonus += 1;
                amountOfErrors = 1;
            }

            points = ((100 / timeInSeconds) * difficulty) / amountOfErrors;

            if (!evaluation)
            {
                points -= 1;
                if (points < 0)
                {
                    return 0;
                }

                return points;
            }

            points++;
            if (points > 9)
            {
                points = 9;
            }

            points += bonus;
            return points;
        }

        /// <summary>
        /// Same method as the Points method now just a different calculation:
        /// "((100/(timeInSeconds/pages))*difficulty)/amountOfErrors"
        /// </summary>
        /// <param name="timeInSeconds"></param>
        /// <param name="amountOfErrors"></param>
        /// <param name="difficulty"></param>
        /// <param name="evaluation"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public float PointsWithPages(float timeInSeconds, float amountOfErrors, float difficulty, bool evaluation,
            int pages)
        {
            float points = 0.0f;
            var bonus = 0;
            if (amountOfErrors == 0)
            {
                bonus += 1;
                amountOfErrors = 1;
            }

            points = ((100 / (timeInSeconds/pages)) * difficulty) / amountOfErrors;

            if (!evaluation)
            {
                points -= 1;
                if (points < 0)
                {
                    return 0;
                }

                return points;
            }

            points++;
            if (points > 9)
            {
                points = 9;
            }

            points += bonus;
            return points;
        }

        #endregion
    
    }
}