using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendingMail : MonoBehaviour
{
    public TextMeshProUGUI _tmp;
    public void SetMail(bool evaluation)
    {
        _tmp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (evaluation)
        {
            _tmp.text = "Thank you!";
        }
        else
        {
            _tmp.text = "I lost all my money!";
        }
        
    }
}
