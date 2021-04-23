using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RotationsSave
{
    [SerializeField, Range(0, 360)] private int _posX;
    [SerializeField, Range(0, 360)] private int _posY;
    [SerializeField] private AudioClip _audio;
    private bool _firstTime = true;

    public RotationsSave(int posX, int posY)
    {
        _posX = posX;
        _posY = posY;
        _firstTime = true;
    }

    #region Getters

    public int GetPosX()
        {
            return _posX;
        }
        public int GetPosY()
        {
            return _posY;
        }
        public AudioClip GetAudio()
        {
            return _audio;
        }
        public bool GetFirst()
        {
            return _firstTime;
        }

    #endregion

    #region Setters

     public void SetFirst(bool first)
     {
         _firstTime = first;
     }   

    #endregion

    
}
