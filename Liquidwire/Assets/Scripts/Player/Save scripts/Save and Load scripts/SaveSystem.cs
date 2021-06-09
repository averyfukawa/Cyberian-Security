using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MissionSystem;
using UI.Browser;
using UI.Browser.Emails;
using UnityEngine;

namespace Player.Save_scripts.Save_and_Load_scripts
{
    public static class SaveSystem
    {
        /// <summary>
        /// Save the all the data: "Email listings, printed pages, open tabs and all the sticky notes"
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="bm"></param>
        /// <param name="listings"></param>
        /// <param name="missionManager"></param>
        public static void SavePlayer(PlayerData playerData, BrowserManager bm, List<EmailListing> listings, 
            MissionManager missionManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "player.save";
            FileStream stream = new FileStream(path, FileMode.Create);
            
            List<PrintStatusSave> printList = new List<PrintStatusSave>();
            foreach (var currentState in bm.GetPrintStatus())
            {
                printList.Add(new PrintStatusSave(currentState.Key, currentState.Value));
            }
            
            PlayerSaveData playerSaveData = new PlayerSaveData();
            playerSaveData.SetStickyNotes(GameObject.FindObjectOfType<HelpStickyManager>().objectListByID);
            playerSaveData.SetPrintedCaseIDs(GameObject.FindObjectOfType<FilingCabinet>().caseFolders);
            playerSaveData.SetLocation(playerData);
            playerSaveData.SetTabs(bm.tabList);
            playerSaveData.SetEmails(listings);
            playerSaveData.SetCreatedCases(missionManager.GetCreated());
            playerSaveData.SetPlayerLevel(missionManager.playerLevel);
            playerSaveData.SetPrintStatus(printList);
            
            formatter.Serialize(stream, playerSaveData);
            stream.Close();
        }
        
        /// <summary>
        /// Load all the saveData from the saveFile.
        /// </summary>
        /// <returns></returns>
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
                Debug.Log("No save file found");
                return null;
            }
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