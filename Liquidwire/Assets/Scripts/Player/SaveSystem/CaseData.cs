using System;

namespace Player
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