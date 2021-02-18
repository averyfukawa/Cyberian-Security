using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace FraudMessage
{
    public class Scenario
    {
        private List<SubScenario> _subScenarios = new List<SubScenario>();
        private int _currentSub = 0;
        private static int _id = 0;
        private int _scenarioId;
        public Scenario()
        {
            _scenarioId = _id;
            _id++;
            _subScenarios.Add(new SubScenario( new Answer(1, "Answer1"),
                new Answer(2, "Answer2"),"hier komt text"));
            _subScenarios.Add(new SubScenario( new Answer(3, "Answer1"),
                new Answer(4, "Answer2"),"hierKomt3"));
            _subScenarios.Add(new SubScenario( new Answer(5, "Answer1"),
                new Answer(6, "Answer2"),"hier kom"));
        }

        public string GetSubText()
        {
            return _subScenarios[_currentSub].GetText();

        }


        public SubScenario GetSub()
        {
            return _subScenarios[_currentSub];
        }

        public void SetCurrentSub(int newSub)
        {
            this._currentSub = newSub;
        }

    }
    
    
}