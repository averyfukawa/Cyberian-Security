using System;

namespace Player
{
    [Serializable]
    public class EmailListingPosition
    {
        private float offsetMaxY;
        private float offsetMinY;

        public EmailListingPosition(float offsetMaxY, float offsetMinY)
        {
            this.offsetMaxY = offsetMaxY;
            this.offsetMinY = offsetMinY;
        }
        public float GetOffsetMaxY()
        {
            return this.offsetMaxY;
        }

        public float getOffsetMinY()
        {
            return this.offsetMinY;
        }
    }
}