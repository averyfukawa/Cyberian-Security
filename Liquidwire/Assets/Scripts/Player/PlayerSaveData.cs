using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerSaveData
    {
        public float[] characterPosition;
        
        public float[] bodyRotation;
        public float[] cameraRotation;


        public PlayerSaveData(PlayerData playerData, GameObject camera)
        {
            characterPosition = new float[3];
            characterPosition[0] = playerData.transform.position.x;
            characterPosition[1] = playerData.transform.position.y;
            characterPosition[2] = playerData.transform.position.z;

            bodyRotation = new float[3];
            bodyRotation[0] = playerData.transform.forward.x;
            bodyRotation[1] = playerData.transform.forward.y;
            bodyRotation[2] = playerData.transform.forward.z;
            
            cameraRotation = new float[3];
            cameraRotation[0] = camera.transform.forward.x;
            cameraRotation[1] = camera.transform.forward.y;
            cameraRotation[2] = camera.transform.forward.z;
        }

        public float GetX()
        {
            return characterPosition[0];
        }

        public float GetY()
        {
            return characterPosition[1];
        }

        public float GetZ()
        {
            return characterPosition[2];
        }
    }
}