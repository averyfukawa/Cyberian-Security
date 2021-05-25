using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using NUnit.Framework;
using Player;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

namespace Editor.NodeV3
{
    public class NodeBasedEditor : EditorWindow
    {
        private List<Node> nodes;
        private List<Connection> connections;

        private List<EmailListingDictionary> _missioncases;
        public List<EmailListingDictionary> inputAbleCases;


        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;
        private GUIStyle inPointStyle;
        private GUIStyle outPointStyle;

        private ConnectionPoint selectedInPoint;
        private ConnectionPoint selectedOutPoint;

        private Vector2 offset;
        private Vector2 drag;

        private float menuBarHeight = 20f;
        private Rect menuBar;

        [MenuItem("Window/Storyline editor")]
        private static void OpenWindow()
        {
            NodeBasedEditor window = GetWindow<NodeBasedEditor>();
            window.titleContent = new GUIContent("Node Based Editor");
        }

        private void OnEnable()
        {
            nodes = new List<Node>();
            inputAbleCases = new List<EmailListingDictionary>();

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            inPointStyle = new GUIStyle();
            inPointStyle.normal.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            inPointStyle.active.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            inPointStyle.border = new RectOffset(4, 4, 12, 12);

            outPointStyle = new GUIStyle();
            outPointStyle.normal.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            outPointStyle.active.background =
                EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            outPointStyle.border = new RectOffset(4, 4, 12, 12);

            _missioncases = FindObjectOfType<SaveCube>().GetComponent<SaveCube>().mailDict;

            
            // loads the save file when a savefile is found
            if (System.IO.File.Exists(Application.dataPath + "/Editor/NodeV3/nodes.xml"))
            {
                this.Load();
            }
        }

        private void OnGUI()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);
            DrawMenuBar();

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawMenuBar()
        {
            menuBar = new Rect(0, 0, position.width, menuBarHeight);

            GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35)))
            {
                Save();
            }

            GUILayout.Space(5);

            if (GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35)))
            {
                Load();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                    new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                    new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        ClearConnectionSelection();
                    }

                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }

                    break;

                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }

                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void DrawConnectionLine(Event e)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawBezier(
                    selectedInPoint.rect.center,
                    e.mousePosition,
                    selectedInPoint.rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    selectedOutPoint.rect.center,
                    e.mousePosition,
                    selectedOutPoint.rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            inputAbleCases = _missioncases.ToList();


            
            // goes through the nodes list ands only get the cases which haven't been added as a node yet. 
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < _missioncases.Count; j++)
                {
                    if (_missioncases[j].listing.GetComponent<EmailListing>().caseName == nodes[i].title)
                    {
                        inputAbleCases.Remove(_missioncases[j]);
                    }
                }
            }
            
            GenericMenu genericMenu = new GenericMenu();
            
            // add context menu for each available cases
            for (int i = 0; i < inputAbleCases.Count; i++)
            {
                int x = i;

                genericMenu.AddItem(
                    new GUIContent("Add Cases / " + GetListingByName(GetCaseNameInINputAbleCases(x)).listingPosition +
                                   " " + GetCaseNameInINputAbleCases(x)), false,
                    () => OnClickAddNode(mousePosition, GetCaseNameInINputAbleCases(x)));
            }
            genericMenu.ShowAsContext();
        }


        private void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }
        
        private void OnClickAddNode(Vector2 mousePosition, string name)
        {
            
            if (nodes == null)
            {
                nodes = new List<Node>();
            }

            nodes.Add(new Node(mousePosition, 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle,
                OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
                GetListingByName(name)));
        }


        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    // prevent looping of storylines
                    if (!selectedInPoint.node._emailListing.isStoryLineStart && selectedInPoint.node._emailListing.prerequisiteMissionId == 0 )
                    {
                        CreateConnection();
                        ClearConnectionSelection();
                    }
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                // prevent looping of storylines
                if (selectedOutPoint.node != selectedInPoint.node && selectedInPoint.node._emailListing.prerequisiteMissionId == 0)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < connections.Count; i++)
                {
                    if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                    {
                        connectionsToRemove.Add(connections[i]);
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    connections.Remove(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }

            nodes.Remove(node);
        }

        /// <summary>
        ///  As soon as a connection gets deleted it will search through the connection list.
        ///  Search for connections connected to this node and see if any conditions need to be changed.
        /// </summary>
        /// <param name="connection"></param>
        private void OnClickRemoveConnection(Connection connection)
        {
            
            foreach (var currentConnection in connections)
            {
                if (currentConnection != connection)
                {
                    //Look at currentConnections inpoint node and compare it to the removed outpoint node if they match it means that there is a connection.
                    if (currentConnection.inPoint.node == connection.outPoint.node)
                    {
                        ChangeConnectionSettingsUponDeletion(currentConnection.inPoint, connection);
                    }

                    if (currentConnection.outPoint.node == connection.inPoint.node)
                    {
                        ChangeConnectionSettingsUponDeletion(currentConnection.outPoint, connection);
                    }
                }
            }
            
            if (!CheckIfSecondConnectionWithNodeInPointExists(connection))
            {
                var temp = connection.inPoint.node._emailListing;
                temp.isStoryMission = false;
                temp.prerequisiteMissionId = 0;
            }
            if (!CheckIfSecondConnectionWithNodeOutPointExists(connection))
            {
                connection.outPoint.node._emailListing.isStoryMission = false;
            }
            
            connection.inPoint.hasConnection = false;
            connection.outPoint.hasConnection = false;
            
            connections.Remove(connection);
        }


        /// <summary>
        /// Checks if the about to be removed connection has nodes attachted to it that are also connected to other nodes. 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private bool CheckIfSecondConnectionWithNodeOutPointExists(Connection connection)
        {
            foreach (var currentConnection in connections)
            {
                if (currentConnection != connection)
                {
                    //Look at currentConnections inpoint node and compare it to the removed outpoint node if they match it means that there is a connection.
                    if (currentConnection.inPoint.node == connection.outPoint.node)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///  Checks if the about to be removed connection has nodes attachted to it that are also connected to other nodes. 

        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private bool CheckIfSecondConnectionWithNodeInPointExists(Connection connection)
        {
            foreach (var currentConnection in connections)
            {
                if (currentConnection != connection)
                {
                    //Look at currentConnections inpoint node and compare it to the removed outpoint node if they match it means that there is a connection.
                    if (currentConnection.outPoint.node == connection.inPoint.node)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Changes the settings after a connection deletion has occured
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="tempConn"></param>
        private void ChangeConnectionSettingsUponDeletion(ConnectionPoint temp, Connection tempConn)
        {
            int counter = 0;
            foreach (var secondConnection in connections)
            {
                Debug.Log(secondConnection.outPoint.node._emailListing.caseName + " vs " + temp.node._emailListing.caseName);
                if (secondConnection.outPoint.node == temp.node)
                {
                    int secondaryCounter = 0;
                    foreach (var doubleCheck in connections)
                    {
                        secondaryCounter++;
                        if (secondConnection.inPoint.node == doubleCheck.outPoint.node)
                        {
                            break;
                        }
                    }
                    if (secondaryCounter == connections.Count)
                    {
                        if (temp.type == ConnectionPointType.Out)
                        {
                            temp.node._emailListing.prerequisiteMissionId = 0;
                        }
                    }
                    break;
                }
                counter++;
            }
            if (counter == connections.Count)
            {
                temp.node._emailListing.isStoryMission = false;
                temp.node._emailListing.prerequisiteMissionId = 0;
            }
        }

        private void CreateConnection()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }
            selectedInPoint.hasConnection = true;
            selectedOutPoint.hasConnection = true;
            connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        private void Save()
        {
            XMLOp.Serialize(nodes, Application.dataPath + "/Editor/NodeV3/nodes.xml");
            XMLOp.Serialize(connections, Application.dataPath + "/Editor/NodeV3/connections.xml");
        }

        private void Load()
        {
            var nodesDeserialized = XMLOp.Deserialize<List<Node>>(Application.dataPath + "/Editor/NodeV3/nodes.xml");
            var connectionsDeserialized =
                XMLOp.Deserialize<List<Connection>>(Application.dataPath + "/Editor/NodeV3/connections.xml");

            nodes = new List<Node>();
            connections = new List<Connection>();

            foreach (var nodeDeserialized in nodesDeserialized)
            {
                nodes.Add(new Node(
                        nodeDeserialized.rect.position,
                        nodeDeserialized.rect.width,
                        nodeDeserialized.rect.height,
                        nodeStyle,
                        selectedNodeStyle,
                        inPointStyle,
                        outPointStyle,
                        OnClickInPoint,
                        OnClickOutPoint,
                        OnClickRemoveNode,
                        nodeDeserialized.inPoint.id,
                        nodeDeserialized.outPoint.id,
                        GetListingByName(nodeDeserialized.title)
                    )
                );
            }

            foreach (var connectionDeserialized in connectionsDeserialized)
            {
                var inPoint = nodes.First(n => n.inPoint.id == connectionDeserialized.inPoint.id).inPoint;
                var outPoint = nodes.First(n => n.outPoint.id == connectionDeserialized.outPoint.id).outPoint;
                connections.Add(new Connection(inPoint, outPoint, OnClickRemoveConnection));
            }
        }

        private String GetCaseNameInINputAbleCases(int index)
        {
            return inputAbleCases[index].listing.GetComponent<EmailListing>().caseName;
        }

        private void LogInputCases()
        {
            string cases = "";

            foreach (var VARIABLE in inputAbleCases)
            {
                cases += VARIABLE.listing.GetComponent<EmailListing>().caseName;
            }

            Debug.Log("inside inputcases: " + cases);
        }

        private void LogMissionCases()
        {
            string cases = "";

            foreach (var VARIABLE in _missioncases)
            {
                cases += VARIABLE.listing.GetComponent<EmailListing>().caseName;
            }

            Debug.Log("inside missioncase: " + cases);
        }

        private EmailListing GetListingByName(string name)
        {
            foreach (var listing in _missioncases)
            {
                if (listing.listing.GetComponent<EmailListing>().caseName == name)
                {
                    return listing.listing.GetComponent<EmailListing>();
                }
            }
            return null;
        }
    }
}