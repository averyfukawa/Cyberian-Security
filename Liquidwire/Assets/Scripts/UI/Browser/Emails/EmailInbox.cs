using System.Collections.Generic;
using Player.Save_scripts.Artificial_dictionaries;
using UnityEngine;

namespace UI.Browser.Emails
{
    public class EmailInbox : MonoBehaviour
    {
        /// <summary>
        /// Transform of the inbox so that the prefab wil be spawned in a proper location
        /// </summary>
        [SerializeField] private Transform _inboxTrans;
        /// <summary>
        /// The list of currently open emails
        /// </summary>
        [SerializeField] private List<EmailListing> _currentEmails = new List<EmailListing>();
        private int _currentCaseNumber;

        #region create Email
        /// <summary>
        /// Create a new email based on the prefab provided.
        /// </summary>
        /// <param name="newListingPrefab"></param>
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

        /// <summary>
        /// Create a method when the player loads a save.
        /// </summary>
        /// <param name="newListingPrefab"></param>
        /// <param name="position"></param>
        /// <param name="status"></param>
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
            newMailRect.offsetMin = new Vector2(newMailRect.offsetMin.x, position.GetOffsetMinY());
        }

        #endregion

        public List<EmailListing> GetEmails()
        {
            return _currentEmails;
        }

        public void Reset()
        {
            _currentEmails = new List<EmailListing>();
        }
    }
}