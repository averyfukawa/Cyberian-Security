using System;

namespace UI.Translation
{
    [Serializable]
    public class ArtificialDictionaryWarning 
    {
        public string objectName;
        public bool allowWarning = true;
        
        public ArtificialDictionaryWarning(string objectName)
        {
            this.objectName = objectName;
        }

        public bool GetWarning()
        {
            return allowWarning;
        }

        public string GetName()
        {
            return objectName;
        }
    }
}
