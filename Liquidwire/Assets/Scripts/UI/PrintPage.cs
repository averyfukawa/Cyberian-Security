using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HoverOverObject), typeof(Rigidbody))]
public class PrintPage : MonoBehaviour
{
    private Rigidbody _rb;
    private HoverOverObject _hOO;
    [SerializeField] private GameObject _fileButton;
    public int caseNumber;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _hOO = GetComponent<HoverOverObject>();
        _hOO.ToggleActive();
        _fileButton.SetActive(false);
    }

    private void Update()
    {
        if (_rb.IsSleeping() && _rb.useGravity)
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _hOO.ToggleActive();
            _hOO.SetOriginPoints();
        }
    }

    public void ToggleButton()
    {
        _fileButton.SetActive(!_fileButton.activeSelf);
    }

    public void FileCase()
    {
        FilingCabinet.Instance.FetchFolderByCase(caseNumber).FilePage(this);
        _fileButton.SetActive(false);
    }
}
