using System;
using UI.Browser.Emails;
using UnityEngine;

namespace Player.Save_scripts.Artificial_dictionaries
{
    [Serializable]
    public class EmailListingDictionary
    {
        private float _id;
        public GameObject listing;

        public float GetId()
        {
            return _id;
        }

        public void SetId()
        {
            _id = listing.GetComponent<EmailListing>().caseNumber;
        }
        
    }
}