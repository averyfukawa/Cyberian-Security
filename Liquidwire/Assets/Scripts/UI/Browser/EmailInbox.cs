using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EmailInbox : MonoBehaviour
{
    [SerializeField] private Transform _inboxTrans;
    [SerializeField] private List<EmailListing> _currentEmails = new List<EmailListing>();
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
        newMailRect.Translate(new Vector3(0, (newMailRect.rect.height) * -(_currentEmails.Count - 1), 0));
    }


    public void LoadEmail(GameObject newListingPrefab, EmailListingPosition position, int status)
    {
        GameObject newEmail = Instantiate(newListingPrefab, _inboxTrans);
        EmailListing newListing = newEmail.GetComponent<EmailListing>();
        newListing.currentStatus = (EmailListing.CaseStatus) status;
        _currentEmails.Add(newListing);
        _currentCaseNumber++;
        newListing.caseNumber = _currentCaseNumber;
        newListing.SetVisuals();
        RectTransform newMailRect = newEmail.GetComponent<RectTransform>();
        newMailRect.offsetMax = new Vector2(newMailRect.offsetMax.x, position.GetOffsetMaxY());
        newMailRect.offsetMin = new Vector2(newMailRect.offsetMin.x, position.getOffsetMinY());
    }
    
    public List<EmailListing> GetEmails()
    {
        return _currentEmails;
    }

    public void Reset()
    {
        _currentEmails = new List<EmailListing>();
    }
}