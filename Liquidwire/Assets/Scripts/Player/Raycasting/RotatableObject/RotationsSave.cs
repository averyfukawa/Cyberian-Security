using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RotationsSave
{
    [SerializeField, Range(0, 360)] private int posX;
    [SerializeField, Range(0, 360)] private int posY;
    [SerializeField] private AudioClip audio;
    private bool _firstTime = true;

    public RotationsSave(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;
        _firstTime = true;
    }

    #region Getters

    public int GetPosX()
        {
            return posX;
        }
        public int GetPosY()
        {
            return posY;
        }
        public AudioClip GetAudio()
        {
            return audio;
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
