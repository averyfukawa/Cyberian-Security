using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Play3DSound : MonoBehaviour
{
    [EventRef] public string selectSound;
    private EventInstance soundEvent;

    public KeyCode pressToPlaySound;

    private void Start()
    {
        soundEvent = RuntimeManager.CreateInstance(selectSound);
    }

    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(soundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        PlaySound();
    }

    private void PlaySound()
    {
        if (Input.GetKey(pressToPlaySound))
        {
            PLAYBACK_STATE fmodPbState;
            soundEvent.getPlaybackState(out fmodPbState);
            if (fmodPbState != PLAYBACK_STATE.PLAYING)
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
