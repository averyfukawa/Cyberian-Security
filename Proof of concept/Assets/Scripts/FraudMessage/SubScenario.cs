namespace FraudMessage
{
    public class SubScenario
    {
        private static int _id = 0;
        private int _scenarioId;
        private string _text;
        private int _nextScenarioA1;
        private int _nextScenarioA2;

        public SubScenario( string text)
        {
            this._text = text;
            this._scenarioId = _id;
            _id++;


        }

        private int GetScenarioID()
        {
            return this._scenarioId;
        }

        public string GetText()
        {
            return this._text;
        }
        


    }
}

