using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace FraudMessage
{
    public class Scenario
    {
        // private ArrayList _subScenarios = new ArrayList();
        private List<SubScenario> _subScenarios = new List<SubScenario>();

        public Scenario()
        {
            _subScenarios.Add(new SubScenario( "hier komt text"));
        }

        public string GetSubText(int index)
        {
            return _subScenarios[index].GetText();

        }


    }
    
    
}