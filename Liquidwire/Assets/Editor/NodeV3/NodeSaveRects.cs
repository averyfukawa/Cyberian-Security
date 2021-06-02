using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.NodeV3
{
    
    [Serializable]
    public class NodeSaveRects
    {
        public Rect  rect;


        public NodeSaveRects(Rect rect)
        {
            this.rect = rect;
        }
    }
}