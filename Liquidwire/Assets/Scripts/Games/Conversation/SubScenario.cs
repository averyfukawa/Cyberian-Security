using System;
using System.Collections.Generic;

namespace Games.Conversation
{
    [Serializable]
    public class SubScenario
    {
        public int scenarioId;
        public string text;
        public Answer nextScenarioA1;
        public Answer nextScenarioA2;
        public bool isEnd;

        private int GetScenarioID()
        {
            return this.scenarioId;
        }

        public string GetText()
        {
            return this.text;
        }

        public List<Answer> GetAnswers()
        {
            List<Answer> answerList = new List<Answer>();
            answerList.Add(nextScenarioA1);
            answerList.Add(nextScenarioA2);
            return answerList;
        }

        public bool GetIsEnd()
        {
            return isEnd;
        }
    }
}

