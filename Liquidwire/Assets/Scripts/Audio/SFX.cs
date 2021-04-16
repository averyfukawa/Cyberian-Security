using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [EventRef]
    private string sfxEvent;

    private bool isPlaying = false;

    [SerializeField]
    private float clipLength;

    private bool rainInstancePlaying;
    private EventInstance rainInstance;

    public void SoundRain()
    {
        if (!rainInstancePlaying)
        {
            Debug.Log("Rain is now playing");
            rainInstance = RuntimeManager.CreateInstance("event:/SFX/Rain");
            rainInstance.start();

            rainInstancePlaying = true;
        }
    }

    public void SoundRainStop()
    {
        if (rainInstancePlaying)
        {
            rainInstancePlaying = false;
            Debug.Log("Stopping rain audio");
            
            rainInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            rainInstance.release();
        }
    }
    
    public void SoundFolderDown()
    {
        Play("SFX/Folder Down");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    public void SoundPageFlip()
    {
        Play("SFX/Page Flip");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    public void SoundPencilUnderline()
    {
        Play("SFX/Pencil Underline");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    public void SoundPencilCircling()
    {
        Play("SFX/Pencil Circling");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    public void SoundMouseClick()
    {
        Play("SFX/Mouse Click");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    public void SoundKeyboard()
    {
        Play("SFX/Keyboard");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    public void SoundTypewriter()
    {
        Play("SFX/Typewriter");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    public void SoundChewingGum()
    {
        Play("SFX/Chewing Gum");
        
        clipLength = 0.1f;
        StartCoroutine(WaitForEnd(clipLength));
    }
    
    #region playRules

    private void Play(string fmodEvent)
    {
        // Debug.Log("Audio is playing: " + fmodEvent);
        if (isPlaying == false)
        {
            sfxEvent = "event:/" + fmodEvent;

            FMODUnity.RuntimeManager.PlayOneShot(sfxEvent, transform.position);

            isPlaying = true;
        }
    }

    private IEnumerator WaitForEnd(float length)
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException("Length of clip is not set in " + " " + gameObject.name +
                                                  nameof(length));

        length = clipLength;
        yield return new WaitForSeconds(length);
        isPlaying = false;
    }

    #endregion
}
