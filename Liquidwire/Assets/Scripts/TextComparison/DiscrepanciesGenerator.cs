using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TextComparison;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class DiscrepanciesGenerator : MonoBehaviour
{

    // private Dictionary<string, List<string>> disc = new Dictionary<string, List<string>>();
    public List<Discrepancie> dcList;
    private static string _path;
    private string _Jsonstring;


    public void start()
    {
        _path = Application.dataPath + "/Scripts/TextComparison/Discrepancies.json";
        _Jsonstring = File.ReadAllText(_path);
        
           dcList = new List<Discrepancie>();


           dcList = JsonUtility.FromJson<DcList>(_Jsonstring).dcList;
        Debug.Log("Dc is now" + dcList.Count);
        Debug.Log("Dc is now" + dcList);
    }
    
    // get inputted string

    public string Discrepamesage(string message)
    {
        
        //todo implement difficulty scaling
        int difficulty;
        
        string[] messageSplit = message.Split(' ');
        
        for (int i = 0; i < messageSplit.Length; i++)
        {
            bool alreadyUsed = false;
            int rdm = GenerateRandomNumber(0, 10);
            
            
            //generate based on difficulty level
            if (rdm >= 8)
            {
                foreach (var VARIABLE in dcList)
                {
                    if (VARIABLE.word.Equals(messageSplit[i]) && !alreadyUsed)
                    {
                        alreadyUsed = true;

                        int index = GenerateRandomNumber(0, VARIABLE.discrepancie.Length - 1);
                        messageSplit[i] = VARIABLE.discrepancie[index];

                    }
                }
            }

        }

        String newMessage = messageSplit.ToString();

        return newMessage;
    }

    private int GenerateRandomNumber(int minValue, int maxValue)
    {
        Random random = new Random();
        int randomNumber = random.Next(minValue, maxValue);

        return randomNumber;

    }
    
    //todo save this horrific named thing.
    
    // split string by space
    
    // loop through array and find words commonly used.
    
    // if match is found replace them with a discrepancie
    
}
