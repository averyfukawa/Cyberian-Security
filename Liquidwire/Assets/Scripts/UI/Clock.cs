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
        _minutesHand.rotation = Quaternion.Euler(FindAngleForFractionOfWhole(_currentTime.Minute, 60f), 0, 0);
        _hoursHand.rotation = Quaternion.Euler(FindAngleForFractionOfWhole(_currentTime.Hour, 12f), 0, 0);
    }

    void Update()
    {
        if (_currentTime.Minute != System.DateTime.Now.Minute)
        { // update clock hands if needed
            _currentTime = System.DateTime.Now;
            if (_currentTime.Minute == 0)
            {
                _hoursHand.LeanRotate(new Vector3(FindAngleForFractionOfWhole(_currentTime.Hour, 12f), 0, 0), .1f);
            }
            _minutesHand.LeanRotate(new Vector3(FindAngleForFractionOfWhole(_currentTime.Minute, 60f), 0, 0), .1f);
        }
    }

    private float FindAngleForFractionOfWhole(float currentValue, float maxValue)
    {
        float fractionPercentage = currentValue / maxValue;
        return 360 * fractionPercentage;
    }
}
