using System;
using System.Xml.Serialization;
using NodeEditorFramework;
using UnityEditor;
using UnityEngine;


namespace Editor.NodeV3
{
    public class Node
    {
        public Rect rect;
        public int caseId;
        
        public string title;
        [XmlIgnore] public bool isDragged;
        [XmlIgnore] public bool isSelected;
        [XmlIgnore] public Rect rectID;


        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;

        [XmlIgnore] public GUIStyle style;
        [XmlIgnore] public GUIStyle defaultNodeStyle;
        [XmlIgnore] public GUIStyle selectedNodeStyle;
        [XmlIgnore] public GUIStyle styleID;

        [XmlIgnore] public EmailListing _emailListing;


        [XmlIgnore] public Action<Node> OnRemoveNode;

        [XmlIgnore] public NodeBasedEditor nodeBasedEditor;

        public Node()
        {
        }
        
        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint,
            Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, EmailListing emailListing, NodeBasedEditor nodeBasedEditor)
        {
            
            float rowHeight = height / 7;
            
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = OnClickRemoveNode;

            rectID = new Rect(position.x, position.y + rowHeight, width, rowHeight);
            styleID = new GUIStyle();
            styleID.alignment = TextAnchor.UpperCenter;

            this._emailListing = emailListing;
            title = _emailListing.caseName ;

            this.nodeBasedEditor = nodeBasedEditor;
        }

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint,
            Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, string inPointID,
            string outPointID, EmailListing emailListing, NodeBasedEditor nodeBasedEditor)
        {
            
            float rowHeight = height / 7;

            rectID = new Rect(position.x, position.y + rowHeight, width, rowHeight);
            styleID = new GUIStyle();
            styleID.alignment = TextAnchor.UpperCenter;
            
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint, inPointID);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, outPointID);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = OnClickRemoveNode;

            this._emailListing = emailListing;
            title = _emailListing.caseName ;

            this.nodeBasedEditor = nodeBasedEditor;

        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
            rectID.position += delta;

        }

        public void Draw()
        {
            inPoint.Draw();
            outPoint.Draw();
            GUI.Box(rect, "", style);
            GUI.Label(rectID, title, styleID);

        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            style = defaultNodeStyle;
                        }
                    }

                    if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }

                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }

                    break;
            }

            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.AddItem(new GUIContent("Story start"), _emailListing.isStoryLineStart, ToggleIsStoryLineStart );
            genericMenu.ShowAsContext();
        }

        private void ToggleIsStoryLineStart()
        {
            _emailListing.isStoryLineStart = !_emailListing.isStoryLineStart;

        }

        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
            {

                this._emailListing.isStoryMission = false;
                this._emailListing.isStoryLineStart = false;
                this._emailListing.prerequisiteMissionId = 0; 
                
                nodeBasedEditor.OnNodeDeletion(this);

                OnRemoveNode(this);
                
            }
        }
    }
}
