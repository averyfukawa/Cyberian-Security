using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class MenuButtons : MonoBehaviour
{
    private PlayerData pd;
    private Movement move;
    private MouseCamera mc;
    [SerializeField] private GameObject menuCam;
    [SerializeField] private RectTransform audioMenu;
    private float _audioRectPosX;

    public void Start()
    {
        pd = FindObjectOfType<PlayerData>();
        move = pd.gameObject.GetComponent<Movement>();
        move.isLocked = true;
        mc = move.gameObject.GetComponentInChildren<MouseCamera>();
        mc.SetCursorNone();
        _audioRectPosX = audioMenu.anchoredPosition.x;
        audioMenu.anchoredPosition = Vector3.zero;
        audioMenu.localScale = Vector3.zero;
    }

    public void Update()
    {
        if (mc.GetLockedState())
        {
            mc.SetCursorNone();
        }
    }

    public void LoadPlayer()
    {
        pd.LoadPlayer();
        StartCoroutine(DoCamTransition());
    }

    public void StartGame()
    {
        StartCoroutine(DoCamTransition());
    }

    public void MenuAudio()
    {
        StartCoroutine(audioMenu.localScale.x <= 0 ? GrowRect(1f) : ShrinkRect(1f));
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        
    }

    private IEnumerator GrowRect(float timeInSec)
    {
        float timer = 0;
        while (timer < timeInSec)
        {
            timer += Time.deltaTime;
            float scaleFactor = timer / timeInSec;
            audioMenu.anchoredPosition = new Vector2(_audioRectPosX*scaleFactor,0);
            audioMenu.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator ShrinkRect(float timeInSec)
    {
        float timer = 0;
        while (timer < timeInSec)
        {
            timer += Time.deltaTime;
            float scaleFactor = timer / timeInSec;
            audioMenu.anchoredPosition = new Vector2(_audioRectPosX - _audioRectPosX*scaleFactor,0);
            audioMenu.localScale = new Vector3(1-scaleFactor, 1-scaleFactor, 1-scaleFactor);
            
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator DoCamTransition()
    {
        mc.SetCursorLocked();
        foreach (var btn in transform.GetComponentsInChildren<Button>())
        {
            btn.gameObject.SetActive(false);
        }
        audioMenu.gameObject.SetActive(false);
        Transform mainCamTrans = Camera.main.transform;
        menuCam.LeanMove(mainCamTrans.position, 1.5f);
        menuCam.LeanRotate(mainCamTrans.rotation.eulerAngles, 1.5f);
        yield return new WaitForSeconds(1.5f);
        move.isLocked = false;
        menuCam.SetActive(false);
    }
}
