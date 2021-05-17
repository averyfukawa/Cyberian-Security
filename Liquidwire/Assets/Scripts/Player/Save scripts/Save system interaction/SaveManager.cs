using System.Collections.Generic;
using Player.Raycasting;
using Player.Save_scripts.Artificial_dictionaries;
using Player.Save_scripts.Save_and_Load_scripts;
using UI.Browser;
using UnityEngine;

namespace Player.Save_scripts.Save_system_interaction
{
    public class SaveManager: MonoBehaviour
    {
        public float theDistance;
        public float maxInteractDistance;
        /// <summary>
        /// List of all the tab prefabs
        /// </summary>
        public List<TabPrefabDictionary> tabdict;
        /// <summary>
        /// List of all the email prefabs
        /// </summary>
        public List<EmailListingDictionary> mailDict;

        private void Start()
        {
            foreach (var item in tabdict)
            {
                item.SetId();
            }
        }

        /// <summary>
        /// Get the length of the cases
        /// </summary>
        /// <param name="caseIndex"></param>
        /// <returns></returns>
        public int GetCaseLength(int caseIndex)
        {
            int count = 0;
            foreach (var tab in tabdict)
            {
                if (caseIndex == Mathf.FloorToInt(tab.GetId()))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// When moused over the gameObject. Check the distance and wait for an interaction.
        /// </summary>
        private void OnMouseOver()
        {
            if (theDistance <= maxInteractDistance)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    PlayerData pd =  FindObjectOfType<PlayerData>();
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