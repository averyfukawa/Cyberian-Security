using System;
using System.Collections.Generic;

namespace Games.Conversation
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

        // debug
        public int GetAmountOfSubScenarios()
        {
            return subScenarios.Count;
        }

        // debug
        public string GetscenarioText()
        {
            int counter = 1;
            String text = "";
            foreach (var sc in subScenarios)
            {
                text += counter + ": " + sc.text + " ";
                counter++;
            }

            return text;
        }

        // debug purposes
        public string GetSubscenarioInfo()
        {
            string text = "";
            int count = 0;
            foreach (var VARIABLE in subScenarios)
            {
                text += "Subscenario1: " + " ";
                text += VARIABLE.text + " ";
                List<Answer> listy = VARIABLE.GetAnswers();

                foreach (var answer in listy)
                {
                    text += answer.ToString() + " ";
                }

                count++;

            }
            return text;
        }

    }
    
    
}