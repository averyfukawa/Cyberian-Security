using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace MissionSystem
{
    /** System that manages the incoming flow of missions.  */
    [Serializable]
    public class MissionManager : MonoBehaviour
    {
        public List<GameObject> missionCases;
        public int gameDifficulty;
        public int maxAmountOfCasesOnDisplay;
        
        private List<EmailListing> _createdMissions;
        private EmailInbox _emailInbox;
        private VirtualScreenSpaceCanvaser _virtualScreenSpaceCanvaser;
        private HoverOverObject  _hoverMonitor;

        private void Start()
        {
            StartCoroutine(WaitForBoot());
        }

        private void Update()
        {
            //todo add this functionality to the system once a mission has been completed.
            if (Input.GetKeyDown("k"))
            {
                
                // checks if the monitor is being used. If it isn't add new missions to the system.                
                if (!_hoverMonitor.GetIsPlaying())
                {
                    
                    AddMissions();
                    Debug.Log("New missions added");
                }
            }
        }

        /** Method that initializes the first x amount of mission  */
        public void InitMission()
        {
            for (int i = 0; i < maxAmountOfCasesOnDisplay; i++)
            {
                GameObject newEmail = Instantiate(missionCases[i], _emailInbox.GetInboxTrans());
                EmailListing newListing = newEmail.GetComponent<EmailListing>();
                _createdMissions.Add(newListing);
                _emailInbox.AddEmail(newListing);
            }
        }

        public void AddMissions()
        {
            //toggle the canvas so it updates outside of the monitor view
            _virtualScreenSpaceCanvaser.ToggleCanvas();
            
            while (_emailInbox.GetEmails().Count < maxAmountOfCasesOnDisplay)
            {
                //loop through entire prefabs
                for (int i = 0; i < missionCases.Count; i++)
                {
                    
                    bool alreadyExists = false;

                    //loop through all the missions
                    for (int j = 0; j < _createdMissions.Count; j++)
                    {
                        
                        // when the mission has already been created do not create it again
                        if (missionCases[i].GetComponent<EmailListing>().listingPosition ==
                            _createdMissions[j].listingPosition)
                        {
                            
                            alreadyExists = true;
                        }
                    }
                    
                    // create new mission when it doesn't exist
                    if (!alreadyExists)
                    {
                        GameObject newEmail = Instantiate(missionCases[i], _emailInbox.GetInboxTrans());
                        EmailListing newListing = newEmail.GetComponent<EmailListing>();
                        _createdMissions.Add(newListing);
                        _emailInbox.AddEmail(newListing);
                        

                    }
                }
            }

            //toggle the canvas so it updates outside of the monitor view
            _virtualScreenSpaceCanvaser.ToggleCanvas();

        }


        //todo implement this. This should be called once a story mission has been completed and the next one has been unlocked.
        private void GetStoryMission()
        {
            throw new NotImplementedException();
        }

        //todo implement this method. This should be called when the user loads his save file.
        /** Loads the mission state when a load is selected */
        private void LoadManagerState()
        {
        }

        private IEnumerator WaitForBoot()
        {
            yield return new WaitForEndOfFrame();

            GameObject gameObject = GameObject.FindWithTag("VSCMonitor");
            
            _hoverMonitor = gameObject.GetComponent<HoverOverObject>();

            _virtualScreenSpaceCanvaser =
                gameObject.GetComponent<VirtualScreenSpaceCanvaser>();

            _emailInbox = FindObjectOfType<EmailInbox>();

            _createdMissions = new List<EmailListing>();

            _virtualScreenSpaceCanvaser.ToggleCanvas();
            
            InitMission();

            //todo implement this
            // LoadManagerState();

            _virtualScreenSpaceCanvaser.ToggleCanvas();
        }

        /** return the EmaiListing based on given casenumber  */
        private EmailListing findListingByCaseNumber(int caseNumber)
        {
            foreach (var mission in _createdMissions)
            {
                if (mission.caseNumber == caseNumber)
                {
                    return mission;
                }
            }

            return null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
        }
    }
}