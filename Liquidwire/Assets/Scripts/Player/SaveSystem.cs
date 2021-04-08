using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Player
{
    public static class SaveSystem
    {
        public static void SavePlayer(PlayerData playerData)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "player.save";
            FileStream stream = new FileStream(path, FileMode.Create);

            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            PlayerSaveData playerSaveData = new PlayerSaveData(playerData, camera);

            Debug.Log("Saving the following: " + playerSaveData.GetX() + " " + playerSaveData.GetY() + " " +
                      playerSaveData.GetZ());

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
                Debug.Log("Loading the following: " + saveData.GetX() + " " + saveData.GetY() + " " + saveData.GetZ());

                stream.Close();
                ;

                return saveData;
            }
            else
            {
                Debug.Log("No save file fouind");
                return null;
            }
        }
       
    }
}