﻿using System;
using UnityEngine;

namespace Player
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

        public void setId()
        {
            _id = listing.GetComponent<EmailListing>().caseNumber;
        }
        
    }
}