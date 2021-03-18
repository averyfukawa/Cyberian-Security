using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextComparison;
using UnityEngine;
using Random = UnityEngine.Random;
[Serializable]
public class DiscrepanciesGenerator : MonoBehaviour
{
    public List<Discrepancie> dcList;
    private static string _path;
    private string _Jsonstring;
    
    public void Start()
    {
        _path = Application.dataPath + "/Scripts/TextComparison/Discrepancies.json";
        _Jsonstring = File.ReadAllText(_path);
        dcList = new List<Discrepancie>();
        
        dcList = JsonUtility.FromJson<DcList>(_Jsonstring).dcList;
    }
    
    // get inputted string

    public string DiscrapeMessage(string message, int difficulty)
    {
        int scamChance = 50;
        
        message = message.Replace("\r", " \r");
        message = message.Replace("\n", " \n");
        
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
                sentenceEnder = returnEndOfSentence(messageSplit[i]);
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
                            int index = Random.Range(0, VARIABLE.discrepancieDictionary.Count);
                            
                            if (VARIABLE.discrepancieDictionary[index].difficulty == difficulty ||
                                VARIABLE.discrepancieDictionary[index].difficulty == (difficulty-1) && difficulty != 1)
                            {
                                messageSplit[i] = VARIABLE.discrepancieDictionary[index].discrepancies;
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

    public String returnEndOfSentence(String word)
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
        }
        else
        {
            return "\r";
        }
    }
    
}