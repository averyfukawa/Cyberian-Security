using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform _minutesHand;
    [SerializeField] private Transform _hoursHand;
    private System.DateTime _currentTime;

    void Start()
    {
        // set clock hands to match system time
        _currentTime = System.DateTime.Now;
        _hoursHand.localRotation = Quaternion.Euler(System.DateTime.Now.IsDaylightSavingTime() ? FindAngleForFractionOfWhole(_currentTime.Hour, 12f) : FindAngleForFractionOfWhole(_currentTime.Hour+1, 12f), 0, 0);
        _minutesHand.localRotation = Quaternion.Euler(FindAngleForFractionOfWhole(_currentTime.Minute, 60f), 0, 0);
        Debug.Log(System.DateTime.Now.Hour);
    }

    void Update()
    {
        if (_currentTime.Minute != System.DateTime.Now.Minute)
        { // update clock hands if needed
            _currentTime = System.DateTime.Now;
            if (_currentTime.Minute == 0)
            {
                _hoursHand.localRotation = Quaternion.Euler(System.DateTime.Now.IsDaylightSavingTime() ? FindAngleForFractionOfWhole(_currentTime.Hour, 12f) : FindAngleForFractionOfWhole(_currentTime.Hour+1, 12f), 0, 0);
            }
            _minutesHand.localRotation = Quaternion.Euler(FindAngleForFractionOfWhole(_currentTime.Minute, 60f), 0, 0);
        }
    }

    /// <summary>
    /// Finds the angle it needs to turn to.
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    private float FindAngleForFractionOfWhole(float currentValue, float maxValue)
    {
        float fractionPercentage = currentValue / maxValue;
        return -360 * fractionPercentage; // the negative value dictates the turning direction
    }
}
