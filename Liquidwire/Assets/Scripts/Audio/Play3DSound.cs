using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play3DSound : MonoBehaviour
{
    [FMODUnity.EventRef] public string selectSound;
    private FMOD.Studio.EventInstance soundEvent;

    public KeyCode pressToPlaySound;

    private void Start()
    {
        soundEvent = FMODUnity.RuntimeManager.CreateInstance(selectSound);
    }

    private void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        PlaySound();
    }

    private void PlaySound()
    {
        if (Input.GetKey(pressToPlaySound))
        {
            FMOD.Studio.PLAYBACK_STATE fmodPbState;
            soundEvent.getPlaybackState(out fmodPbState);
            if (fmodPbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                soundEvent.start();
            }
        }

        if (Input.GetKeyUp(pressToPlaySound))
        {
            soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
