using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    private FMOD.Studio.VCA vcaControl;
    public string vcaName;

    [SerializeField] private float vcaVolume;
    
    private Slider volumeSlider;
    
    // Start is called before the first frame update
    private void Start()
    {
        vcaControl = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        volumeSlider = GetComponent<Slider>();

        vcaControl.getVolume(out vcaVolume);
    }

    public void SetVolume(float volume)
    {
        vcaControl.setVolume(volume);
        
        vcaControl.getVolume(out vcaVolume);
    }
}
