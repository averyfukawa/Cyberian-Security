using System;
using System.Collections;
using System.Collections.Generic;
using FraudMessage;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{

    private List<Scenario> _scenarios = new List<Scenario>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        _scenarios.Add(new Scenario());
        Debug.Log(_scenarios[0].GetSubText(0));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
