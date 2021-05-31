using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.NodeV3
{
    
    [Serializable]
    public class NodeSaveData
    {

        public List<NodeSaveRects> rects; 

        public NodeSaveData(List<NodeSaveRects> rect)
        {
            this.rects = rect;
        }
    }
}