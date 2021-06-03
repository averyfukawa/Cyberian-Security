﻿using System;
using System.Collections.Generic;
using Player.Save_scripts.Artificial_dictionaries;
using UI.Browser.Emails;
using UI.Browser.Tabs;
using UnityEngine;

namespace Player.Save_scripts.Save_and_Load_scripts
{
    [Serializable]
    public class PlayerSaveData
    {
        /// <summary>
        /// an array of different axis's of the character position
        /// </summary>
        public float[] characterPosition;
        /// <summary>
        /// an array of different axis's of the character rotation
        /// </summary>
        public float[] bodyRotation;
        /// <summary>
        /// A list of the currently open tabs
        /// </summary>
        public List<float> tabList;
        /// <summary>
        /// A list of the info of the open tabs
        /// </summary>
        public List<SaveInfo> tabInfoList;
        /// <summary>
        /// List of all the open emaillistings
        /// </summary>
        public List<int> mailListings;
        /// <summary>
        /// save the status of the open emaillistings
        /// </summary>
        public List<int> mailStatus;
        /// <summary>
        /// a list of the selected stickyIds
        /// </summary>
        public List<int> stickyIds;
        /// <summary>
        /// saves the location of the emaillistings.
        /// </summary>
        public List<EmailListingPosition> emailPosition;
        /// <summary>
        /// Save the currently printed case ids
        /// </summary>
        public List<float> printedCaseIDs;
        
        public List<int> createdCases;

        #region Saving

        /// <summary>
        /// Set all the information from the Emails into the saveData.
        /// </summary>
        /// <param name="listings"></param>
        public void SetEmails(List<EmailListing> listings)
        {
            emailPosition = new List<EmailListingPosition>();
            mailListings = new List<int>();
            mailStatus = new List<int>();
            foreach (var item in listings)
            {
                mailListings.Add(item.listingPosition);
                mailStatus.Add((int) item.currentStatus);
                
                emailPosition.Add( new EmailListingPosition(
                    item.gameObject.GetComponent<RectTransform>().offsetMax.y,
                    item.gameObject.GetComponent<RectTransform>().offsetMin.y));
            }
        }

        /// <summary>
        /// Set all the currently printed caseIds into the saveData
        /// </summary>
        /// <param name="tempList"></param>
        public void SetPrintedCaseIDs(List<CaseFolder> tempList)
        {
            printedCaseIDs = new List<float>();
            foreach (var caseItem in tempList)
            {
                foreach (var pagesItem in caseItem.GetPagesL())
                {
                    printedCaseIDs.Add(pagesItem.caseFileId);
                }
            }
        }
        
        /// <summary>
        /// Set all currently active tabs into the saveData.
        /// </summary>
        /// <param name="tabList"></param>
        public void SetTabs(List<Tab> tabList)
        {
            this.tabList = new List<float>();
            tabInfoList = new List<SaveInfo>();
            foreach (var item in tabList)
            {
                var temp = item.tabInfo;
                tabInfoList.Add(new SaveInfo(temp.tabHeadText, temp.tabURL, temp.isSecure, temp.caseNumber));
                this.tabList.Add(item.tabId);
            }
        }

        /// <summary>
        /// Set the current player location into the saveData.
        /// </summary>
        /// <param name="playerData"></param>
        public void SetLocation(PlayerData playerData)
        {
            characterPosition = new float[3];
            characterPosition[0] = playerData.transform.position.x;
            characterPosition[1] = playerData.transform.position.y;
            characterPosition[2] = playerData.transform.position.z;
            
            bodyRotation = new float[3];
            bodyRotation[0] = playerData.transform.forward.x;
            bodyRotation[1] = playerData.transform.forward.y;
            bodyRotation[2] = playerData.transform.forward.z;
        }
        
        /// <summary>
        /// Set all currently active tabs into the saveData.
        /// </summary>
        /// <param name="stickyList"></param>
        public void SetStickyNotes(List<HelpStickyObject> stickyList)
        {
            stickyIds = new List<int>();
            foreach (var item in stickyList)
            {
                if (item.isStickied)
                {
                    stickyIds.Add(item.stickyID);
                }
            }
            
        }
        
        public void SetCreatedCases(List<EmailListing> createdListings)
        {
            createdCases = new List<int>();
            foreach (var listing in createdListings)
            {
                createdCases.Add(listing.listingPosition);
            }
        }
        
        #endregion

        #region Getting

        public List<float> GetPrinted()
        {
            return printedCaseIDs;
        }
        
        public float GetX()
        {
            return characterPosition[0];
        }

        public float GetY()
        {
            return characterPosition[1];
        }

        public float GetZ()
        {
            return characterPosition[2];
        }

        public List<int> getCreatedList()
        {
            return createdCases;
        }

        #endregion
        
    }
}