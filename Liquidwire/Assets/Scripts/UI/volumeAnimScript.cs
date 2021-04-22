using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumeAnimScript : MonoBehaviour
{
    [SerializeField] private Animation volumeAnim;

    public void PlayOpenAnim()
    {
        volumeAnim.Play("anim_volume_slide");
    }

    public void PlayCloseAnim()
    {
        volumeAnim.Play("anim_volume_slideBackwards");
    }
}
