using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    private VCA vcaControl;
    public string vcaName;

    [SerializeField] private float vcaVolume;
    private Slider volumeSlider;

    private float savedVolMaster;
    private float savedVolMusic;
    private float savedVolSfx;
    
    private void Start()
    {
        vcaControl = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        volumeSlider = GetComponent<Slider>();
        
        vcaControl.getVolume(out vcaVolume);

        switch (vcaName)
        {
            case "Master":
                savedVolMaster = PlayerPrefs.GetFloat("MasterVol");
                vcaControl.setVolume(savedVolMaster);
                volumeSlider.value = savedVolMaster;
                break;
            
            case "Music":
                savedVolMusic = PlayerPrefs.GetFloat("MusicVol");
                vcaControl.setVolume(savedVolMusic);
                volumeSlider.value = savedVolMusic;
                break;
            
            case "SFX":
                savedVolSfx = PlayerPrefs.GetFloat("SFXVol");
                vcaControl.setVolume(savedVolSfx);
                volumeSlider.value = savedVolSfx;
                break;
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        vcaControl.setVolume(volume);
        
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("MasterVol", volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        vcaControl.setVolume(volume);
        
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }
    
    public void SetSfxVolume(float volume)
    {
        vcaControl.setVolume(volume);
        
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }
}