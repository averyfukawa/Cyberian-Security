using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SolvedArtDictionary
{
    private bool _hasWon;
    private bool _outcome;
    private float _id;
    public SolvedArtDictionary(bool hasWon, bool outcome, float id)
    {
        _hasWon = hasWon;
        _outcome = outcome;
        _id = id;
    }

    public bool GetHasWon()
    {
        return _hasWon;
    }

    public bool GetOutcome()
    {
        return _outcome;
    }

    public float GetId()
    {
        return _id;
    }
}
