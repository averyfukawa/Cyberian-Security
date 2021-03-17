using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKM : MonoBehaviour
{
    [Header("Menu")]
    [FMODUnity.EventRef]
    [SerializeField]
    private string musicMenu = "event:/BKM/BKM";

    [Space]
    [Header("In-game")]
    [FMODUnity.EventRef]
    [SerializeField]
    private string musicGamePlay = "event:/BKM/BKM";

    private FMOD.Studio.EventInstance instanceSong;
    
    public void MenuMusic()
    {
        instanceSong = FMODUnity.RuntimeManager.CreateInstance(musicMenu);
        instanceSong.start();
    }
    
    private IEnumerator WaitForEnd(float length)
    {
        yield return new WaitForSeconds(length);
        instanceSong.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        // Transition FX if needed
        instanceSong.release();
    }
}
