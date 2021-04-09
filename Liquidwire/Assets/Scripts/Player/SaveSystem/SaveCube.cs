using System;
using UnityEngine;

namespace Player
{
    public class SaveCube: MonoBehaviour
    {

        public float theDistance;
        public float maxInteractDistance;
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