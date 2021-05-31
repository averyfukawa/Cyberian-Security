using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Editor.NodeV3
{

    public class Connection
    {
        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        [XmlIgnore] public Action<Connection> OnClickRemoveConnection;

        public Connection()
        {
        }

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;

            // if (inPoint.node._emailListing != null && outPoint.node._emailListing )
            inPoint.node._emailListing.SetStoryLine(outPoint.node._emailListing.listingPosition);
            inPoint.node._emailListing.LogConnection();
            outPoint.node._emailListing.isStoryMission = true;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                inPoint.rect.center,
                outPoint.rect.center,
                inPoint.rect.center + Vector2.left * 50f,
                outPoint.rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8,
                Handles.RectangleCap))
            {
                
                if (OnClickRemoveConnection != null)
                {

                    OnClickRemoveConnection(this);

                }
            }
        }
    }
}