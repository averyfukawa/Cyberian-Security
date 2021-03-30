using System;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    [SerializeField]
    private DialogManager _dialogManager;
    public int DialogIdToLoad = 0;
    public float value = 0.1f;
    bool once = true;

    void Start()
    {
        //DialogBlackboard.SetValue(DialogBlackboard.EDialogMultiChoiceVariables.TryingThisToo, value);
       
    }

    void Update()
    {
        //Load
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     value += 0.1f;
        //     DialogBlackboard.SetValue(DialogBlackboard.EDialogMultiChoiceVariables.TryingThisToo, value);
        // }

        ////Data
        //if (Input.GetKey(KeyCode.D))
        //{
        //    //BaseDialogNode node = _dialogManager.GetNodeForID(DialogIdToLoad);
        //    //Debug.Log("Character name : " + node.SayingCharacterName);
        //    //Debug.Log("Character says : " + node.WhatTheCharacterSays);
        //    //Debug.Log("Character portrait name : " + node.SayingCharacterPotrait.name);
        //}

        ////Next
        //if (Input.GetKey(KeyCode.N))
        //{
        //    //_dialogManager.GiveInputToDialog(DialogIdToLoad, EDialogInputValue.Next);
        //}
    }

    public void StartDialog()
    {
        _dialogManager.ShowDialogWithId(DialogIdToLoad, true);
    }

    public void SetDialog(DialogManager dm)
    {
        _dialogManager = dm;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position,10);
    }
}
