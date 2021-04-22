using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class volumeAnimScript : MonoBehaviour
{
    [SerializeField] private Animation volumeAnim;
    private bool isOpen;
    [SerializeField] private GameObject clickableSpace;

    private void Start()
    {
        if (clickableSpace.activeSelf)
        {
            clickableSpace.SetActive(false);
        }
    }

    public void PlayOpenAnim()
    {
        volumeAnim.Play("anim_volume_slide");
        clickableSpace.SetActive(true);
    }

    public void PlayCloseAnim()
    {
        volumeAnim.Play("anim_volume_slideBackwards");
        clickableSpace.SetActive(false);
    }

    public void ClosePanelWhenClickedOutside()
    {
        PlayCloseAnim();
        Debug.Log("Clicked");
    }
}
