using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailInbox : MonoBehaviour
{
    [SerializeField] private Transform _inboxTrans;
    private List<EmailListing> _currentEmails = new List<EmailListing>();
    private int _currentCaseNumber;

    public void NewEmail(GameObject newListingPrefab)
    {
        GameObject newEmail = Instantiate(newListingPrefab, _inboxTrans);
        EmailListing newListing = newEmail.GetComponent<EmailListing>();
        _currentEmails.Add(newListing);
        _currentCaseNumber++;
        newListing.caseNumber = _currentCaseNumber;
        newListing.SetVisuals();
        RectTransform newMailRect = newEmail.GetComponent<RectTransform>();
        newMailRect.Translate(new Vector3(0,(newMailRect.rect.height)*-(_currentEmails.Count-1),0));
    }
    
}
