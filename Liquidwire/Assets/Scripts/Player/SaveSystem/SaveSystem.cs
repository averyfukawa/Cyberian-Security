using System;
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

            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            PlayerSaveData playerSaveData = new PlayerSaveData(playerData, bm.tabList, listings);
            SaveCases(playerSaveData);
            
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
                
                LoadCases(saveData.cases, saveData.tabList);
                
                stream.Close();

                return saveData;
            }
            else
            {
                Debug.Log("No save file fouind");
                return null;
            }
        }

        public static void SaveCases(PlayerSaveData playerSaveData)
        {
            EmailInbox emailInbox = GameObject.FindObjectOfType<EmailInbox>();
           List<EmailListing> emails = emailInbox.GetEmails();

           List<CaseData> cases = new List<CaseData>();
           foreach (var mail in emails)
           {
               
               CaseData caseData = new CaseData(mail.currentStatus, mail.caseNumber);
               cases.Add(caseData);

           }

           playerSaveData.SetCasesList(cases);
           
        }

        public static void LoadCases(List<CaseData> cases, List<float> tabList)
        {
            EmailInbox emailInbox = GameObject.FindObjectOfType<EmailInbox>();
            BrowserManager bm = GameObject.FindObjectOfType<BrowserManager>();
        }

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