using System;
using System.Collections.Generic;
using System.IO;
using Games.TextComparison.Artificial_dictionary_scripts;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Games.TextComparison.Discrepancy_generators
{
    [Serializable]
    public class DiscrepancyGenerator : MonoBehaviour
    {
        public List<Discrepancy> dcList;
        private static string path;
        private string _jsonString;
    
        public void Start()
        {
            path = Application.dataPath + "/Scripts/Games/TextComparison/Discrepancies.json";
            _jsonString = File.ReadAllText(path);
            dcList = new List<Discrepancy>();
        
            dcList = JsonUtility.FromJson<DcList>(_jsonString).dcList;
        }
        
        /// <summary>
        /// This method will create a discrepancy based on the message and the difficulty provided
        /// </summary>
        /// <param name="message"></param>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public string DiscrepancyMessage(string message, int difficulty)
        {
            int scamChance = 50;
        
            /*
             * Loop through the message split over the space. Then create a random Int to see if there will be a scam
             * 
             */
            string[] messageSplit = message.Split(' ');

            for (int i = 0; i < messageSplit.Length; i++)
            {
                bool alreadyUsed = false;
                bool sentenceEnd = false;
                int rng = Random.Range(0, 100);
                String sentenceEnder = "";
            
                //checks if there are any spaces or !?. in it, rabobank, abn bank
                if (messageSplit[i].Contains(".") || messageSplit[i].Contains(",") || messageSplit[i].Contains("\r") || 
                    messageSplit[i].Contains("?") || messageSplit[i].Contains("!") )
                {
                    sentenceEnder = ReturnEndOfSentence(messageSplit[i]);
                    messageSplit[i] =  messageSplit[i].Replace(sentenceEnder, "");
                    sentenceEnd = true;
                }
            
                if (rng >= scamChance)
                {
                    foreach (var VARIABLE in dcList)
                    {
                        if (VARIABLE.word.ToLower().Equals(messageSplit[i].ToLower()) && !alreadyUsed)
                        {
                            int counter = 0;
                            while (counter < 10)
                            {
                                int index = Random.Range(0, VARIABLE.discrepancyDictionary.Count);
                            
                                if (VARIABLE.discrepancyDictionary[index].difficulty == difficulty ||
                                    VARIABLE.discrepancyDictionary[index].difficulty == (difficulty-1) && difficulty != 1)
                                {
                                    messageSplit[i] = VARIABLE.discrepancyDictionary[index].discrepancies;
                                    scamChance += 10;
                                    alreadyUsed = true;
                                    break;
                                }
                                counter++;
                            }
                        }
                    }
                }
            
                if (sentenceEnd)
                {
                    messageSplit[i] += sentenceEnder;
                    scamChance -= 10;
                }

                messageSplit[i] += " ";
            }
        
            String newMessage = String.Concat(messageSplit);

            return newMessage;
        }
        
        /// <summary>
        /// Returns the end of the sentence as a String.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public String ReturnEndOfSentence(String word)
        {
            if (word.Contains("."))
            {
                return ".";
            } else if (word.Contains("?"))
            {
                return "?";
            } else if (word.Contains("!"))
            {
                return "!";
            } else if (word.Contains(","))
            {
                return ",";
            } else if (word.Contains("\n"))
            {
                return "\n";
            }
            else
            {
                return "\r";
            }
        }
    
    }
}