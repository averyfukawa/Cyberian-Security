using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    private string og;



    public void Start()
    {
        _path = Application.dataPath + "/Scripts/TextComparison/Discrepancies.json";
        _Jsonstring = File.ReadAllText(_path);
        dcList = new List<Discrepancie>();
        og = "hello and to rabobank Wij zijn er bedanktsaam voor je samenwerking en geachte je zeer om weg te gaan.";
        Debug.Log("OG  = " + og );


        dcList = JsonUtility.FromJson<DcList>(_Jsonstring).dcList;
    }

    public void TestCase()
    {

        Debug.Log(DiscrapeMessage(og).ToString());
    }
    
    // get inputted string

    public string DiscrapeMessage(string message)
    {
        //todo implement difficulty scaling
        int difficulty;
        
        string[] messageSplit = message.Split(' ');
        
        for (int i = 0; i < messageSplit.Length; i++)
        {
            Debug.Log("Checking " + messageSplit[i]);
            bool alreadyUsed = false;
            int rdm = GenerateRandomNumber(0, 10);
            
            
            //generate based on difficulty level
            if (rdm >= 0)
            {
                foreach (var VARIABLE in dcList)
                {
                    if (VARIABLE.word.Equals(messageSplit[i]) && !alreadyUsed)
                    {
                        Debug.Log("Discrep found in word " + messageSplit[i]);
                        alreadyUsed = true;

                        int index = GenerateRandomNumber(0, VARIABLE.discrepancie.Length - 1);
                        messageSplit[i] = VARIABLE.discrepancie[index];


                    }
                }
            }

            messageSplit[i] += " ";

        }

        String newMessage = String.Concat(messageSplit);
        Debug.Log("new message is " + newMessage); 

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