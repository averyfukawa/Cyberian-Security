﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Player;
using Player.Raycasting;
using Player.Save_scripts.Artificial_dictionaries;
using Player.Save_scripts.Save_and_Load_scripts;
using Player.Save_scripts.Save_system_interaction;
using UI.Browser.Emails;
using UnityEngine;
using UnityEngine.Rendering;

namespace MissionSystem
{
    /** System that manages the incoming flow of missions.  */
    [Serializable]
    public class MissionManager : MonoBehaviour
    {
        /// <summary>
        /// Current player level which influences which cases become available
        /// </summary>
        public float playerLevel;
        public int maxAmountOfCasesOnDisplay;
        
        private List<EmailListingDictionary> _missionCases;
        /// <summary>
        /// All the created listings
        /// </summary>
        private List<EmailListing> _createdMissions;
        private EmailInbox _emailInbox;
        private VirtualScreenSpaceCanvaser _virtualScreenSpaceCanvaser;
        private HoverOverObject _hoverMonitor;
        private void Start()
        {
            StartCoroutine(WaitForBoot());
        }

        private void Update()
        {
            if (Input.GetKeyDown("k") && TutorialManager.Instance.testMode)
            {
                // checks if the monitor is being used. If it isn't add new missions to the system.                
                if (!_hoverMonitor.GetPlaying())
                {
                    Debug.Log("K pressed and !over " + _hoverMonitor.GetPlaying());
                    FindAndAddMission();
                }
            }
        }

        /** Method that initializes the first x amount of mission  */
        public void InitMission()
        {
            // adds 1 mission from the list
            for (int i = 0; i < 1; i++)
            {
                AddMissionToInbox(_missionCases[i].listing);
            }
        }

        #region Add mission methods
        /// <summary>
        ///  Adds a new mission based on the player's level
        /// </summary>
        public void FindAndAddMission()
        {
            _virtualScreenSpaceCanvaser.ToggleCanvas();

            //get currentlistofMissions
            List<EmailListing> listings = _emailInbox.GetEmails();

            // check how many of each level there are  ( dictionary<diff,  amount>)
            Dictionary<int, int> currentAmountBasedOnDifficulty = this.GetAmountOfMissionsPerDifficulty(listings);

            bool lowerDisabled = false;
            int lower = 0;

            if (Mathf.RoundToInt(playerLevel) == 1)
            {
                //disables lower when the player level is 1 because there are no difficulty 0 missions
                lowerDisabled = true;
            }
            else
            {
                lower = Mathf.RoundToInt(playerLevel - 1);
            }

            // current level
            int currentLevel = Mathf.RoundToInt(playerLevel);

            // 1 level higher than current level
            int higher = Mathf.RoundToInt(playerLevel + 1);

            // amount of missions in emailbox with difficulty  playerlevel -1 
            int amountDown = 0;

            // amount of missions in emailbox with difficulty  playerlevel 
            int amountAt = 0;

            // amount of missions in emailbox with difficulty  playerlevel + 1 
            int amountUp = 0;


            if (_emailInbox.GetEmails().Count >= maxAmountOfCasesOnDisplay)
            {
                Debug.Log("Max amount of missions in box reached");
            }
            else
            {
                // goes through each difficulty key and set the value of the amount of difficulty missions in the email boxing
                foreach (var dif in currentAmountBasedOnDifficulty)
                {
                    if (!lowerDisabled && dif.Key == lower)
                    {
                        amountDown = dif.Value;
                    }

                    else if (dif.Key == currentLevel)
                    {
                        amountAt = dif.Value;
                    }
                    else if (dif.Key == higher)
                    {
                        amountUp = dif.Value;
                    }
                    else
                    {
                        Debug.Log(" no same key found");
                    }
                }

                //add mission with diff lower
                if (!lowerDisabled && amountDown < 1)
                {
                    AddMissionBasedOnDifficulty(lower);
                }
                // add mission with diff same as playerlevel;
                else if (amountAt < 2)
                {
                    AddMissionBasedOnDifficulty(currentLevel);
                }
                // add mission based on diff  1 up;
                else if (amountUp < 1)
                {
                    AddMissionBasedOnDifficulty(higher);
                }
                //When there is no available difficulty to add. Get the first available mission and add it.
                else
                {
                    Debug.Log("Getting`leftover ");
                    AddNextInLineMission();
                }
            }

            _virtualScreenSpaceCanvaser.ToggleCanvas();
        }


        /// <summary>
        /// Adds the next available case into the emailinbox.
        /// </summary>
        public void AddNextInLineMission()
        {
            int count = 0;
            for (int i = 0; i < _missionCases.Count; i++)
            {
                bool alreadyExists = false;

                //loop through all the missions
                for (int j = 0; j < _createdMissions.Count; j++)
                {
                    // when the mission has already been created do not create it again
                    if (_missionCases[i].listing.GetComponent<EmailListing>().listingPosition ==
                        _createdMissions[j].listingPosition)
                    {
                        alreadyExists = true;
                    }
                }

                if (!alreadyExists)
                {
                    // check storymission
                    if (_missionCases[i].listing.GetComponent<EmailListing>().isStoryMission)
                    {
                        if (HasCompletedPrerequisiteMission(_missionCases[i].listing.GetComponent<EmailListing>()
                            .prerequisiteMissionId))
                        {
                            AddMissionToInbox(_missionCases[i].listing);
                            return;
                        }
                    }
                    else
                    {
                        AddMissionToInbox(_missionCases[i].listing);
                        return;
                    }
                }
                else
                {
                    count++;
                }

                if (count == _missionCases.Count)
                {
                    //todo  rig this into congratulations you've completed em all
                    Debug.Log("all missions have been made");
                    CheckAllMissionCompletion();
                }
            }
        }
        
        /// <summary>
        ///  Adds a new case to the emailinbox based on given difficulty
        /// </summary>
        /// <param name="difficulty"></param>
        public void AddMissionBasedOnDifficulty(int difficulty)
        {
            List<EmailListingDictionary> availableMissions = new List<EmailListingDictionary>();

            for (int i = 0; i < _missionCases.Count; i++)
            {
                bool alreadyExists = false;

                //loop through all the missions
                for (int j = 0; j < _createdMissions.Count; j++)
                {
                    // when the mission has already been created do not create it again
                    if (_missionCases[i].listing.GetComponent<EmailListing>().listingPosition ==
                        _createdMissions[j].listingPosition)
                    {
                        alreadyExists = true;
                    }
                }

                // create new mission when it doesn't exist
                if (!alreadyExists)
                {
                    //checks if the mission is available based on given difficulty

                    if (_missionCases[i].listing.GetComponent<EmailListing>().difficultyValue == difficulty)
                    {
                        // check storyline
                        if (_missionCases[i].listing.GetComponent<EmailListing>().isStoryMission)
                        {
                            if (HasCompletedPrerequisiteMission(_missionCases[i].listing.GetComponent<EmailListing>()
                                .prerequisiteMissionId))
                            {
                                // add mission to list 
                                availableMissions.Add(_missionCases[i]);
                            }
                        }
                        else
                        {
                            // add mission to list 
                            availableMissions.Add(_missionCases[i]);
                        }
                    }
                }
            }

            // When no available missions are found get the next available mission ignoring difficulty
            if (availableMissions.Count == 0)
            {
                Debug.Log("no missions with difficulty " + difficulty + " found adding a leftover");
                AddNextInLineMission();
            }
            // inject mission into game 
            else
            {
                AddMissionToInbox(availableMissions[0].listing);
            }
        }


        /// <summary>
        ///  Adds the given gameobject's emaillisting to the emailinbox
        /// </summary>
        /// <param name="currentGameObject"></param>
        private void AddMissionToInbox(GameObject currentGameObject)
        {
            GameObject newEmail = Instantiate(currentGameObject, _emailInbox.GetInboxTrans());
            EmailListing newListing = newEmail.GetComponent<EmailListing>();
            _createdMissions.Add(newListing);
            _emailInbox.AddEmail(newListing);
            Debug.Log(newListing.caseName + " added");
        }
        

        #endregion
        
        #region Check methods

        /** Checks to see if the available missions is available based on current storyline. */
        private bool HasCompletedPrerequisiteMission(int prerequisiteId)
        {
            foreach (var mission in _createdMissions)
            {

                if (mission.listingPosition == prerequisiteId &&
                    mission.currentStatus == EmailListing.CaseStatus.Conclusion)
                {
                    Debug.Log("prereq has been completed you are receiving this story mission");

                    return true;
                }
            }
            Debug.Log("prereq not completed you are denied complete the preq first");
            return false;
        }

        /// <summary>
        /// Checks if all the created missions have the status conclusion
        /// </summary>
        private void CheckAllMissionCompletion()
        {
            int amountOfCompletedMission = 0; 
            foreach (var mission in _createdMissions)
            {
                if (mission.currentStatus == EmailListing.CaseStatus.Conclusion) amountOfCompletedMission++;
            }

            if (amountOfCompletedMission == _createdMissions.Count)
            {
                Debug.Log("Congratulations you've solved on the available cases!!!");
            }
            
        }

        #endregion

        #region Get methods

        /** return the EmaiListing that are already created based on given casenumber  */
        private EmailListing FindListingByCaseNumber(int caseNumber)
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

        
        /// <summary>
        ///  counts the amount of missions each difficulty has
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        private Dictionary<int, int> GetAmountOfMissionsPerDifficulty(List<EmailListing> inbox)
        {
            Dictionary<int, int> difficultyAmount = new Dictionary<int, int>();
            
            //todo make it scaleable?
            int one = 0;
            int two = 0;
            int three = 0;
            int four = 0;
            int five = 0;

            foreach (var listing in inbox)
            {
                Debug.Log("Listing diff is " + listing.difficultyValue);
                switch (listing.difficultyValue)
                {
                    case 1:
                    {
                        one++;
                        break;
                    }
                    case 2:
                    {
                        two++;
                        break;
                    }
                    case 3:
                    {
                        three++;
                        break;
                    }
                    case 4:
                    {
                        four++;
                        break;
                    }
                    case 5:
                    {
                        five++;
                        break;
                    }
                }
            }

            difficultyAmount.Add(1, one);
            difficultyAmount.Add(2, two);
            difficultyAmount.Add(3, three);
            difficultyAmount.Add(4, four);
            difficultyAmount.Add(5, five);
            return difficultyAmount;
        }

        private Dictionary<int, int> GetAmountOfMissionsByDifficultyBasedOnPlayerLevel()
        {
            Dictionary<int, int> test = new Dictionary<int, int>();

            switch (Mathf.RoundToInt(playerLevel))
            {
                case 1:
                {
                    test.Add(1, 3);
                    test.Add(2, 1);
                    break;
                }
                case 2:
                {
                    test.Add(1, 1);
                    test.Add(2, 2);
                    test.Add(3, 1);
                    break;
                }
            }

            return test;
        }

        public List<EmailListing> GetCreated()
        {
            return _createdMissions;
        }

        #endregion
        
        /** Loads the mission state when a load is selected */
        public void LoadManagerState(List<int> createdIds)
        {
            foreach (var id in createdIds)
            {
                _createdMissions.Add(_missionCases[id].listing.GetComponent<EmailListing>());
            }
        }

        /** Sets up the missionManager after boot  */
        private IEnumerator WaitForBoot()
        {
            yield return new WaitForEndOfFrame();
            GameObject monitorObject = GameObject.FindWithTag("VSCMonitor");

            _hoverMonitor = monitorObject.GetComponent<HoverOverObject>();

            _virtualScreenSpaceCanvaser =
                monitorObject.GetComponent<VirtualScreenSpaceCanvaser>();

            _missionCases = FindObjectOfType<SaveManager>().GetComponent<SaveManager>().mailDictList;
            _emailInbox = FindObjectOfType<EmailInbox>();

            Debug.Log("While loading inbox has " + -_emailInbox.GetEmails().Count);

            _createdMissions = new List<EmailListing>();

            _virtualScreenSpaceCanvaser.ToggleCanvas();

            _virtualScreenSpaceCanvaser.ToggleCanvas();
        }
        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
        }

        public int FindDifficultyByCaseNumber(int caseNumber)
        {
            foreach (var currentCase in _missionCases)
            {
                var currentEmail = currentCase.listing.GetComponent<EmailListing>();
                if (currentEmail.caseNumber == caseNumber)
                {
                    return currentEmail.difficultyValue;
                }
            }

            return 1;
        }
    }
}