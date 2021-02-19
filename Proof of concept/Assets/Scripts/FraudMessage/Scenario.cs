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
            _subScenarios.Add(new SubScenario( new Answer(1, "Take my money"),
                new Answer(2, "Ik ken u niet"),"hier komt text"));
            _subScenarios.Add(new SubScenario( new Answer(3, "Answer1"),
                new Answer(4, "Ik weet niet waar u het over hebt, kunt u dat herhlaen"),"maar ik heb al neiks"));
            _subScenarios.Add(new SubScenario( new Answer(5, "Answer1"),
                new Answer(6, "Answer2"),"hier kom"));
            _subScenarios.Add(new SubScenario("I got all ya money. hahahaa"));
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