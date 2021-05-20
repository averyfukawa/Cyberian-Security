using System;
using UnityEngine;

namespace UI.Translation
{
    [Serializable]
    public class ArtificialDictionaryWarning 
    {
        public string objectName;
        public bool allowWarning = true;
        public GameObject tmpObject;
        
        public ArtificialDictionaryWarning(string objectName, GameObject tmpObject)
        {
            this.objectName = objectName;
            this.tmpObject = tmpObject;
        }

        public bool GetWarning()
        {
            return allowWarning;
        }

        public GameObject GetObject()
        {
            return tmpObject;
        }

        public string GetName()
        {
            return objectName;
        }
    }
}
