using System;
using UnityEngine;

namespace MissionSystem
{
    [Serializable]
    public class Mission : MonoBehaviour
    {
        public GameObject MissionPrefab;

        public EmailListing mail;
        //todo defineer waar een missie uit bestaat
        // reward etc etc
        private void Start()
        {
            throw new NotImplementedException();
        }
    }
    
    
    
    public enum MissionStatus
    {
        Unopened,
        Started,
        Conclusion
    }

}