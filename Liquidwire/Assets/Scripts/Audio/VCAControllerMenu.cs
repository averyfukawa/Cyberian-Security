using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using FMOD.Studio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VCAControllerMenu : MonoBehaviour
{
    private VCA vcaControl;
    public string vcaName;

    [SerializeField] private float vcaVolume;
    [SerializeField] private Slider[] volumeSliders = new Slider[3];
    [SerializeField] private TextMeshProUGUI[] volumeShowers = new TextMeshProUGUI[3];

    private float savedVolMaster;
    private float savedVolMusic;
    private float savedVolSfx;
    
    private void Start()
    {
        vcaControl = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        
        vcaControl.getVolume(out vcaVolume);

        switch (vcaName)
        {
            case "Master":
                savedVolMaster = PlayerPrefs.GetFloat("MasterVol", 1);
                vcaControl.setVolume(savedVolMaster);
                foreach (var slider in volumeSliders)
                {
                    slider.value = savedVolMaster;
                }

                foreach (var shower in volumeShowers)
                {
                    shower.text = "Master Volume: " + (savedVolMaster*100).ToString("F0");
                }
                break;
            
            case "Music":
                savedVolMusic = PlayerPrefs.GetFloat("MusicVol", 1);
                vcaControl.setVolume(savedVolMusic);
                foreach (var slider in volumeSliders)
                {
                    slider.value = savedVolMusic;
                }
                foreach (var shower in volumeShowers)
                {
                    shower.text = "Music Volume: " + (savedVolMaster*100).ToString("F0");
                }
                break;
            
            case "SFX":
                savedVolSfx = PlayerPrefs.GetFloat("SFXVol", 1);
                vcaControl.setVolume(savedVolSfx);
                foreach (var slider in volumeSliders)
                {
                    slider.value = savedVolSfx;
                }
                foreach (var shower in volumeShowers)
                {
                    shower.text = "SFX Volume: " + (savedVolMaster*100).ToString("F0");
                }
                break;
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        vcaControl.setVolume(volume);
        foreach (var shower in volumeShowers)
        {
            shower.text = "Master Volume: " + (volume*100).ToString("F0");
        }
        foreach (var slider in volumeSliders)
        {
            slider.value = volume;
        }
        
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("MasterVol", volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        vcaControl.setVolume(volume);
        foreach (var shower in volumeShowers)
        {
            shower.text = "Music Volume: " + (volume*100).ToString("F0");
        }
        foreach (var slider in volumeSliders)
        {
            slider.value = volume;
        }
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }
    
    public void SetSfxVolume(float volume)
    {
        vcaControl.setVolume(volume);
        foreach (var shower in volumeShowers)
        {
            shower.text = "SFX Volume: " + (volume*100).ToString("F0");
        }
        foreach (var slider in volumeSliders)
        {
            slider.value = volume;
        }
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }
}