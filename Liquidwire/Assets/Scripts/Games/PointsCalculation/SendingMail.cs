using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendingMail : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public void SetMail(bool evaluation)
    {
        tmp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (evaluation)
        {
            tmp.text = "Thank you!";
        }
        else
        {
            tmp.text = "I lost all my money!";
        }
        
    }
}
