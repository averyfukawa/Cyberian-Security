using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace FraudMessage
{
    [Serializable]
    public class Scenario
    {
        public List<SubScenario> subScenarios;
        public int currentSub = 0;
        public int scenarioId;

        public string GetSubText()
        {
            return subScenarios[currentSub].GetText();

        }


        public SubScenario GetSub()
        {
            return subScenarios[currentSub];
        }

        public void SetCurrentSub(int newSub)
        {
            this.currentSub = newSub;
        }

    }
    
    
}