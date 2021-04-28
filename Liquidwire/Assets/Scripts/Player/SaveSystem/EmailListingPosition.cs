using System;

namespace Player
{
    [Serializable]
    public class EmailListingPosition
    {
        private float _offsetMaxY;
        private float _offsetMinY;

        public EmailListingPosition(float offsetMaxY, float offsetMinY)
        {
            this._offsetMaxY = offsetMaxY;
            this._offsetMinY = offsetMinY;
        }
        public float GetOffsetMaxY()
        {
            return this._offsetMaxY;
        }

        public float GetOffsetMinY()
        {
            return this._offsetMinY;
        }
    }
}