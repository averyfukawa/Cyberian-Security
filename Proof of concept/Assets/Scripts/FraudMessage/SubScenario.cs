using System.Collections.Generic;

namespace FraudMessage
{
    public class SubScenario
    {
        private static int _id = 0;
        private int _scenarioId;
        private string _text;
        private Answer _nextScenarioA1;
        private Answer _nextScenarioA2;

        public SubScenario(Answer answerA, Answer answerB, string text)
        {
            this._text = text;
            this._scenarioId = _id;
            _id++;
            _nextScenarioA1 = answerA;
            _nextScenarioA2 = answerB;
        }

        private int GetScenarioID()
        {
            return this._scenarioId;
        }

        public string GetText()
        {
            return this._text;
        }

        public List<Answer> GetAnswers()
        {
            List<Answer> answerList = new List<Answer>();
            answerList.Add(_nextScenarioA1);
            answerList.Add(_nextScenarioA2);
            return answerList;
        }

    }
}

