using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UI.Browser.Emails;
using UnityEngine;

namespace MissionSystem
{
    [Serializable]

    //** */
    public class MissionManager : MonoBehaviour
    {
        public List<GameObject> MissionCases;
        public List<EmailListing> availableMissions;
        public List<EmailListing> completedMissions;

        private void Start()
        {
            StartCoroutine(WaitForBoot());
            
            
            availableMissions = new List<EmailListing>();
            completedMissions = new List<EmailListing>();

        }

        private void Update()
        {
            if (Input.GetKeyUp("k"))
            {
                InitMission();
            }
        }

        public void InitMission()
        {
            EmailInbox emailInbox = FindObjectOfType<EmailInbox>();
            
            emailInbox.NewEmail(MissionCases[0]);
            emailInbox.NewEmail(MissionCases[1]);
            emailInbox.NewEmail(MissionCases[2]);
        }


        /** Loads the mission state  */
        private void LoadMission()
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
        }

        private IEnumerator WaitForBoot()
        {
            yield return new WaitForEndOfFrame();
            
            GameObject gameObject = GameObject.FindWithTag("VSCMonitor");

            VirtualScreenSpaceCanvaser virtualScreenSpaceCanvaser =
                gameObject.GetComponent<VirtualScreenSpaceCanvaser>();

            virtualScreenSpaceCanvaser.ToggleCanvas();

            InitMission();

            virtualScreenSpaceCanvaser.ToggleCanvas();
        }
    }
}