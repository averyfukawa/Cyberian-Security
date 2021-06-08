using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using FMOD.Studio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    private VCA vcaControl;
    public string vcaName;

    [SerializeField] private float vcaVolume;
    private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeShower;

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
                savedVolMaster = PlayerPrefs.GetFloat("MasterVol", 100);
                vcaControl.setVolume(savedVolMaster);
                volumeSlider.value = savedVolMaster;
                volumeShower.text = "Master Volume: " + (savedVolMaster*100).ToString("F0");
                break;
            
            case "Music":
                savedVolMusic = PlayerPrefs.GetFloat("MusicVol", 100);
                vcaControl.setVolume(savedVolMusic);
                volumeSlider.value = savedVolMusic;
                volumeShower.text = "Music Volume: " + (savedVolMusic*100).ToString("F0");
                break;
            
            case "SFX":
                savedVolSfx = PlayerPrefs.GetFloat("SFXVol", 100);
                vcaControl.setVolume(savedVolSfx);
                volumeSlider.value = savedVolSfx;
                volumeShower.text = "SFX Volume: " + (savedVolSfx*100).ToString("F0");
                break;
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        vcaControl.setVolume(volume);
        volumeShower.text = "Master Volume: " + (volume*100).ToString("F0");
        
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("MasterVol", volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        vcaControl.setVolume(volume);
        volumeShower.text = "Music Volume: " + (volume*100).ToString("F0");
        
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }
    
    public void SetSfxVolume(float volume)
    {
        vcaControl.setVolume(volume);
        volumeShower.text = "SFX Volume: " + (volume*100).ToString("F0");
        
        vcaControl.getVolume(out vcaVolume);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }
}