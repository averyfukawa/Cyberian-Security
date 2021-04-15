using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerSaveData
    {
        public float[] characterPosition;
        public float[] bodyRotation;
        public List<CaseData> cases;
        public List<float> tabList;
        public List<SaveInfo> tabInfoList;
        public List<int> mailListings;
        public List<int> mailStatus;
        public List<int> stickyIds;
        public List<EmailListingPosition> emailPosition;


        //todo makkelijke van prefabs ophalen? 
        
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
        
        public void SaveStickyNotes(List<HelpStickyObject> stickyList)
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
    }
}