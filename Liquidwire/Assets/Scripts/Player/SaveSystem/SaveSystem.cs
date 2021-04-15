﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Player
{
    public static class SaveSystem
    {
        public static void SavePlayer(PlayerData playerData, BrowserManager bm, List<EmailListing> listings)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "player.save";
            FileStream stream = new FileStream(path, FileMode.Create);
            
            PlayerSaveData playerSaveData = new PlayerSaveData();
            playerSaveData.SaveStickyNotes(GameObject.FindObjectOfType<HelpStickyManager>().objectListByID);
            //SaveCases(playerSaveData);
            playerSaveData.SetLocation(playerData);
            playerSaveData.SetTabs(bm.tabList);
            playerSaveData.SetEmails(listings);
            
            formatter.Serialize(stream, playerSaveData);
            stream.Close();
        }

        public static PlayerSaveData LoadPlayer()
        {
            string path = Application.persistentDataPath + "player.save";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                
                PlayerSaveData saveData = formatter.Deserialize(stream) as PlayerSaveData;

                stream.Close();

                return saveData;
            }
            else
            {
                Debug.Log("No save file fouind");
                return null;
            }
        }

        
        // //todo zijn ze nog nodig? Bjarne: "Yeet em"
        // public static void SaveCases(PlayerSaveData playerSaveData)
        // {
        //     EmailInbox emailInbox = GameObject.FindObjectOfType<EmailInbox>();
        //    List<EmailListing> emails = emailInbox.GetEmails();
        //
        //    List<CaseData> cases = new List<CaseData>();
        //    foreach (var mail in emails)
        //    {
        //        
        //        CaseData caseData = new CaseData(mail.currentStatus, mail.caseNumber);
        //        cases.Add(caseData);
        //
        //    }
        //
        //    playerSaveData.SetCasesList(cases);
        //    
        // }


        //todo create method for writing and loading bytes
        // public static void  WriteToByes(string fileName)
        // {
        //     BinaryFormatter formatter = new BinaryFormatter();
        //     string path = Application.persistentDataPath + fileName;
        //     FileStream stream = new FileStream(path, FileMode.Create);
        //     
        //     
        //     formatter.Serialize(stream, playerSaveData);
        //     stream.Close();
        //
        //
        // }
    }
}