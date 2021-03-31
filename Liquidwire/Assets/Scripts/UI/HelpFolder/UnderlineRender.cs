using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderlineRender : MonoBehaviour
{
    private GameObject[] _linesByID = new GameObject[10];
    private int _currentPage;
    [SerializeField] private List<GameObject> _linePages;
    [SerializeField] private GameObject _linePagePrefab;
    [SerializeField] private GameObject _lineHeadPrefab;
    [SerializeField] private GameObject _linePrefab;

    public void Setup(int pageCount)
    {
        for (int i = 0; i < pageCount; i++)
        {
            // fill the list of pages with the correct number of transform children
            _linePages.Add(Instantiate(_linePagePrefab, transform));
        }
    }

    public void MovePage(bool isForward)
    {
        if (isForward)
        {
            _linePages[_currentPage].SetActive(false);
            _currentPage++;
            if (_currentPage == _linePages.Count)
            {
                _currentPage = 0;
            }
            _linePages[_currentPage].SetActive(true);
        }
        else
        {
            _linePages[_currentPage].SetActive(false);
            _currentPage--;
            if (_currentPage < 0)
            {
                _currentPage = _linePages.Count-1;
            }
            _linePages[_currentPage].SetActive(true);
        }
    }

    public void CreateLines(Vector3[] lineCoords, int pageNumber, int ID)
    {
        Destroy(_linesByID[ID]);
        // create the header (which is the object used to toggle on and off by children)
        _linesByID[ID] = Instantiate(_lineHeadPrefab, _linePages[pageNumber - 1].transform);
        // fill in the line values
        for (int i = 0; i < lineCoords.Length/2; i++)
        {
            LineRenderer lR = Instantiate(_linePrefab, _linesByID[ID].transform).GetComponent<LineRenderer>();
            lR.positionCount = 2;
            lR.SetPosition(0, lineCoords[i*2]);
            lR.SetPosition(1, lineCoords[i*2+1]);
        }
    }

    public void DropLines()
    {
        foreach (var line in _linesByID)
        {
           Destroy(line); 
        }
    }

    public void DestroyLine(int ID)
    {
        Destroy(_linesByID[ID]);
    }
}