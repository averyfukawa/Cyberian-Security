using System;
using UI.Browser.Emails;

namespace Player.Save_scripts.Artificial_dictionaries
{
    [Serializable]
    public class CaseData
    {
        public EmailListing.CaseStatus status;
        public int caseId;

        public CaseData(EmailListing.CaseStatus status, int id)
        {
            this.status = status;
            this.caseId = id;
        }
    }
}