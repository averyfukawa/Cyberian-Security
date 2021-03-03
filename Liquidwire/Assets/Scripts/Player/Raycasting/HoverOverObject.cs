using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverObject : MonoBehaviour
{
    public float theDistance;
    public float maxDistance;
    public GameObject textField;

    public virtual void Start()
    {
        textField = GameObject.FindGameObjectWithTag("HoverText");
    }

    // Update is called once per frame
    void Update()
    {
        theDistance = RayCasting.distanceTarget;
    }

    public virtual void OnMouseOver()
    {
        if (theDistance < maxDistance)
        {
            textField.SetActive(true);
            textField.GetComponent<Text>().text = "Use";
            
            if (Input.GetButtonDown("Action"))
            {
            }
        } else if (theDistance > maxDistance && theDistance < (maxDistance+0.5))
        {
            textField.SetActive(false);
        }
    }

    void OnMouseExit()
    {
        if (textField.activeSelf)
        {
            textField.SetActive(false);
        }
    }
}