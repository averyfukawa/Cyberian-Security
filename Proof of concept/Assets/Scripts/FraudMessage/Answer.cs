using System;

namespace FraudMessage
{
    [Serializable]
    public class Answer
    {
        public int answerId;
        public int nextScenario;
        public string answerText;

        public int Get_nextScenario()
        {
            return this.nextScenario;
        }
        
        public string Get_answerText()
        {
            return this.answerText;
        }
    }
}