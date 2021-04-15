using System;
using System.Collections.Generic;
using UI.Browser;
using UnityEngine;

namespace Player
{
    public class SaveCube: MonoBehaviour
    {

        public float theDistance;
        public float maxInteractDistance;
        public List<TabPrefabDictionary> tabdict;
        public List<EmailListingDictionary> mailDict;

        private void Start()
        {
            foreach (var item in tabdict)
            {
                item.SetId();
            }
        }

        private void OnMouseOver()
        {

            if (theDistance <= maxInteractDistance)
            {


                if (Input.GetMouseButtonDown(0))
                {

                    PlayerData pd =  FindObjectOfType<PlayerData>();
                    //
                    pd.SavePlayer();




                }

                if (Input.GetMouseButtonDown(1))
                {
                    PlayerData pd =  FindObjectOfType<PlayerData>(); 

                    pd.LoadPlayer();
                    
                }

            }
        }
        
        void Update()
        {
            theDistance = RayCasting.distanceTarget;
        }
    }
}