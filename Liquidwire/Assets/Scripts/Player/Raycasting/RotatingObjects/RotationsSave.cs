using System;
using System.Collections.Generic;
using UI.Translation;
using UnityEngine;

namespace Player.Raycasting.RotatingObjects
{
    [Serializable]
    public class RotationsSave
    {
        [SerializeField, Range(0, 360)] private int posX;
        [SerializeField, Range(0, 360)] private int posY;

        /// <summary>
        /// Piece of audio that can be played when you enter the current rotation.
        /// </summary>
        [SerializeField] private AudioClip audio;
        private bool _firstTime = true;
        [SerializeField] private List<TranslationObject> translations;

        public RotationsSave(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
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

        public string GetText(LanguageScript.Languages current)
        {
            foreach (var language in translations)
            {
                if (language.language == current)
                {
                    return language.translation;
                }
            }

            return "Null";
        }

        #endregion

        #region Setters

        public void SetFirst(bool first)
        {
            _firstTime = first;
        }   

        #endregion

    
    }
}
