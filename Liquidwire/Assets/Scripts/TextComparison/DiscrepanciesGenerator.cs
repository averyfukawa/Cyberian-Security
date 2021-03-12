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

    public string DiscrapeMessage(string message)
    {
        //todo implement difficulty scaling
        int difficulty;
        int scamChance = 50;

        Debug.Log("old message " + message);
        message = message.Replace("\n", " \n");
        Debug.Log("new message " + message);
        
        string[] messageSplit = message.Split(' ');

        for (int i = 0; i < messageSplit.Length; i++)
        {
            bool alreadyUsed = false;
            bool sentenceEnd = false;
            int rng = Random.Range(0, 100);
            
            //generate based on difficulty level
            //todo fix \n and others.
            if (messageSplit[i].Contains(".")|| messageSplit[i].Contains("\n") ||messageSplit[i].Contains("?") || messageSplit[i].Contains("!") )
            {
                Debug.Log(messageSplit[i] + "dot contained ");

                messageSplit[i] =  messageSplit[i].Replace(".", "");
                sentenceEnd = true;
            }
            
            if (rng >= scamChance)
            {
                foreach (var VARIABLE in dcList)
                {
                    if (VARIABLE.word.ToLower().Equals(messageSplit[i].ToLower()) && !alreadyUsed)
                    {
                        alreadyUsed = true;

                        int index = Random.Range(0, VARIABLE.discrepancie.Length);
                        messageSplit[i] = VARIABLE.discrepancie[index];
                        scamChance += 10;
                    }
                }
            }
            
            if (sentenceEnd)
            {
                Debug.Log("dot gaining at  " +  messageSplit[i]);
                messageSplit[i] += ".";
                Debug.Log("has a dot " + messageSplit[i]);
                scamChance -= 10;
            }

            messageSplit[i] += " ";
        }
        
        String newMessage = String.Concat(messageSplit);

        return newMessage;
    }
    
}