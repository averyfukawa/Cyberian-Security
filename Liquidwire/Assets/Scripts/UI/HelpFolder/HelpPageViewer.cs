using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class HelpPageViewer : MonoBehaviour
{
    [SerializeField] private GameObject[] _pageButtons = new GameObject[2];
    [SerializeField] private UnderlineRender _underLiner;
    [SerializeField] private Transform _documentPosition;
    [SerializeField] private Transform _fileWaypoint;
    [SerializeField] private Image _labelHidingMask;
    public Queue<GameObject> pages = new Queue<GameObject>();
    public List<GameObject> pagesL = new List<GameObject>();

    private void Start()
    {
        ToggleButtons(false);
        if (_labelHidingMask != null)
        {
            _labelHidingMask.enabled = false;
        }

        if (pages.Count == 0)
        {
            foreach (var page in pagesL)
            {
                pages.Enqueue(page);
            }
        }
    }

    public void ToggleButtons(bool enable)
    {
        if (enable)
        {
            for (int i = 0; i < 2; i++)
            {
                _pageButtons[i].SetActive(true);
            }
        }
        else
        {
            foreach (var button in _pageButtons)
            {
                button.SetActive(false);
            }
        }
    }

    public TMP_LinkInfo[] FetchLinkInfos()
    {
        List<TMP_LinkInfo> returnList = new List<TMP_LinkInfo>();
        GameObject[] pageArray = pagesL.ToArray();
        foreach (var page in pageArray)
        {
            TMP_TextInfo info = page.GetComponentInChildren<TextMeshProUGUI>().textInfo;
            returnList = returnList.Concat(info.linkInfo.ToList()).ToList();
        }

        return returnList.ToArray();
    }

    public int CurrentPageNumber()
    {
        for (int i = 0; i < pagesL.Count; i++)
        {
            if (pagesL[i] == pages.Peek())
            {
                return i;
            }
        }
        return -1;
    }

    public void ChangePage(bool isForward)
    {
        if (isForward)
        {
            FlipPage(true);
            _underLiner.MovePage(true);
        }
        else
        {
            FlipPage(false);
            _underLiner.MovePage(false);
        }
    }
    
    #region PageShuffeling

    private void FlipPage(bool forwards)
        {
            if (forwards)
            {
                GameObject oldFrontPage = pages.Dequeue();
                pages.Enqueue(oldFrontPage);
                StartCoroutine(PageFlipAnimationForwards(oldFrontPage.transform, 0.5f));
            }
            else
            {
                GameObject[] tempArray = pages.ToArray();
                GameObject oldFrontPage = pages.Peek();
                GameObject tempValue = tempArray[pages.Count - 1];
                for (int i = 0; i < tempArray.Length; i++)
                {
                    GameObject temp = tempArray[i];
                    tempArray[i] = tempValue;
                    tempValue = temp;
                }
                pages = new Queue<GameObject>(tempArray);
                StartCoroutine(PageFlipAnimationBackwards(oldFrontPage.transform, 0.5f));
            }
        }
        
        private IEnumerator PageFlipAnimationBackwards(Transform oldPageTransform, float animationTime)
        {
            oldPageTransform.LeanMove(FilePositionByIndex(pages.Count-1), 0.01f);
            pages.Peek().transform.LeanMove(_fileWaypoint.position, animationTime*.5f);
            yield return new WaitForSeconds(animationTime*.1f);
            _labelHidingMask.enabled = true;
            yield return new WaitForSeconds(animationTime*.4f);
            pages.Peek().transform.LeanMove(FilePositionByIndex(0), animationTime*.5f);
            yield return new WaitForSeconds(animationTime*.4f);
            _labelHidingMask.enabled = false;
        }
    
        private IEnumerator PageFlipAnimationForwards(Transform oldPageTransform, float animationTime)
        {
            pages.Peek().transform.LeanMove(FilePositionByIndex(0), animationTime*.2f);
            oldPageTransform.LeanMove(_fileWaypoint.position, animationTime*.5f);
            yield return new WaitForSeconds(animationTime*.1f);
            _labelHidingMask.enabled = true;
            yield return new WaitForSeconds(animationTime*.4f);
            oldPageTransform.LeanMove(FilePositionByIndex(pages.Count-1), animationTime*.5f);
            yield return new WaitForSeconds(animationTime*.4f);
            _labelHidingMask.enabled = false;
        }
    
        private Vector3 FilePositionByIndex(int fileIndex)
        {
            Vector3 basePosition = _documentPosition.position;
            Vector3 offset = -_documentPosition.forward * ((fileIndex - pages.Count) * .0001f);
            return basePosition + offset;
        }
    
        public void FilePage(GameObject pageToFile)
        {
            pages.Enqueue(pageToFile);
            pagesL.Add(pageToFile);
            var transform1 = pageToFile.transform;
            transform1.SetParent(_documentPosition, true);
            transform1.position = FilePositionByIndex(pages.Count);
            transform1.localRotation = Quaternion.Euler(new Vector3(0,180,Random.Range(-5f, 5f)));
        }

        public void EmptyFolder()
        {
            if (_documentPosition.childCount > 1)
            {
                Debug.Log("Discarding old File objects");
                while (_documentPosition.childCount > 1)
                {
                    // Debug.Log("destroying object number " + i);
                    DestroyImmediate(_documentPosition.GetChild(1).gameObject);
                }
            }
            pages = new Queue<GameObject>();
            pagesL = new List<GameObject>();
        }

        public void SortFrontToBack()
        {
            for (int i = 0; i < pagesL.Count; i++)
            {
                pagesL[i].transform.position = FilePositionByIndex(i);
            }
        }

    #endregion
}
