namespace FraudMessage
{
    public class Answer
    {
        private static int _id = 0;
        private int _answerId;
        private int _nextScenario;
        private string _answerText;
        
        public Answer(int _nextScenario, string _answerText)
        {
            _answerId = _id;
            this._nextScenario = _nextScenario;
            this._answerText = _answerText;

            _id++;
        }

        public int Get_nextScenario()
        {
            return this._nextScenario;
        }
        
        public string Get_answerText()
        {
            return this._answerText;
        }
    }
}