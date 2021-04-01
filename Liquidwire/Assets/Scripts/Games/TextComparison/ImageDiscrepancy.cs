using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageDiscrepancy : MonoBehaviour
{
    public bool isScam = false;
    private bool _isSelected = false;
    [SerializeField] private Image _selectionCircle;

    private void Start()
    {
        _selectionCircle.fillAmount = 0;
        _selectionCircle.gameObject.SetActive(false);
    }

    public void ChangeSelected()
    {
        _isSelected = !_isSelected;
        if (_isSelected)
        {
            _selectionCircle.gameObject.SetActive(true);
            StartCoroutine(AnimateCircling(.3f));
        }
        else
        {
            _selectionCircle.fillAmount = 0;
            // TODO add shader for eraser here (probably using a mask running through a shadered texture ?)
            _selectionCircle.gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimateCircling(float time)
    {
        float timeSpent = 0;
        while (_selectionCircle.fillAmount < 1)
        {
            timeSpent += Time.deltaTime;
         
            _selectionCircle.fillAmount = timeSpent / time;
            yield return new WaitForEndOfFrame();
        }
    }

    public bool check()
    {
        return _isSelected == isScam;
    }

    public void ResetSelected()
    {
        _isSelected = !_isSelected;
        _selectionCircle.fillAmount = 0;
        // TODO add shader for eraser here (probably using a mask running through a shadered texture ?)
        _selectionCircle.gameObject.SetActive(false);
    }


    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawCube(transform.position, GetComponent<RectTransform>().sizeDelta);
    // }
}
