using System;
using System.Collections;
using System.Collections.Generic;
using Player.Raycasting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FilingCabinet : MonoBehaviour
{
    [SerializeField] private GameObject _caseFolderPrefab;
    
    public static FilingCabinet Instance;
    /// <summary>
    /// List with all the existing caseFolders
    /// </summary>
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

    #region Create folder

    /// <summary>
    /// Create a folder for the opened emaillisting.
    /// </summary>
    /// <returns></returns>
    public CaseFolder CreateFolder()
    {
        // TODO add better placement script here
        GameObject newFolderObj = Instantiate(_caseFolderPrefab, transform.position, Quaternion.Euler(new Vector3(270, 0, Random.Range(0f, 360f))));
        CaseFolder newFolder = newFolderObj.GetComponent<CaseFolder>();
        caseFolders.Add(newFolder);
        return newFolder;
    }
    
    /// <summary>
    /// The create folder method for when the player loads a save.
    /// </summary>
    /// <returns></returns>
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

    #endregion
    
}
