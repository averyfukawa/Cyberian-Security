using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaseGrading
{
    private readonly Dictionary<int, int> _difficultyCounters;
    private SendingMail _sendingMail;
    public CaseGrading()
    {
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