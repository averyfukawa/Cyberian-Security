using System.Collections.Generic;
using Player.Raycasting;
using Player.Save_scripts.Artificial_dictionaries;
using Player.Save_scripts.Save_and_Load_scripts;
using UI.Browser;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Save_scripts.Save_system_interaction
{
    public class SaveManager: MonoBehaviour
    {
        public float theDistance;
        public float maxInteractDistance;
        /// <summary>
        /// List of all the tab prefabs
        /// </summary>
        [FormerlySerializedAs("tabdict")] public List<TabPrefabDictionary> tabDictList;
        /// <summary>
        /// List of all the email prefabs
        /// </summary>
        [FormerlySerializedAs("mailDict")] public List<EmailListingDictionary> mailDictList;

        private void Start()
        {
            foreach (var item in tabDictList)
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
            foreach (var tab in tabDictList)
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
                    if (!FindObjectOfType<PlayerData>().isInViewMode)
                    {
                        PlayerData pd =  FindObjectOfType<PlayerData>();
                        pd.SavePlayer();
                    }
                }
            }
        }
        
        void Update()
        {
            theDistance = RayCasting.distanceTarget;
        }
    }
}