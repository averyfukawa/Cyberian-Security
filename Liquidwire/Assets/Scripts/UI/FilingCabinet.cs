using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FilingCabinet : MonoBehaviour
{
    [SerializeField] private GameObject _caseFolderPrefab;
    
    public static FilingCabinet Instance;
    public List<CaseFolder> caseFolders = new List<CaseFolder>();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public CaseFolder FetchFolderByCase(int caseNumber)
    {
        foreach (var folder in caseFolders)
        {
            if (folder.caseNumber == caseNumber)
            {
                return folder;
            }
        }
        Debug.Log("Failed to find folder");
        return null;
    }

    public CaseFolder CreateFolder()
    {
        // TODO add better placement script here
        GameObject newFolderObj = Instantiate(_caseFolderPrefab, transform.position, Quaternion.Euler(new Vector3(270, 0, Random.Range(0f, 360f))));
        CaseFolder newFolder = newFolderObj.GetComponent<CaseFolder>();
        caseFolders.Add(newFolder);
        return newFolder;
    }
    
    public CaseFolder CreateFolderLoad()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
        GameObject newFolderObj = Instantiate(_caseFolderPrefab, hit.point, Quaternion.Euler(new Vector3(270, 0, Random.Range(0f, 360f))));
        CaseFolder newFolder = newFolderObj.GetComponent<CaseFolder>();
        caseFolders.Add(newFolder);
        
        Rigidbody rb = newFolder.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        newFolder.GetComponent<HoverOverObject>().SetOriginPoints();
        return newFolder;
    }
}
