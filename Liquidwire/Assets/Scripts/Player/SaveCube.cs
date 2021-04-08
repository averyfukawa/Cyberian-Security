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

                    Debug.Log("saved");



                }

                if (Input.GetMouseButtonDown(1))
                {
                    PlayerData pd =  FindObjectOfType<PlayerData>(); 

                    pd.LoadPlayer();
                    
                    Debug.Log("loaded");
                }

            }
        }
        
        void Update()
        {
            theDistance = RayCasting.distanceTarget;
        }
    }
}