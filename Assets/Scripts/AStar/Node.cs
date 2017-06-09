using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool Walkable;
    public int G, H;
    public int GridX, GridY;
    public Vector3 WorldPosition;
    public bool Expensive;
    private Node _parent;

    public Node(bool walkable, Vector3 worldPos, int gridx, int gridy, bool expensive)
    {
        Walkable = walkable;
        WorldPosition = worldPos;
        GridX = gridx;
        GridY = gridy;
        Expensive = expensive;
    }

    public int F
    {
        get
        {
            return G + H;
        }
    }
    public Node Parent
    {
        get { return _parent; }
        set
        {
            _parent = value;
           // G = _parent.G + PathFinder.GetTraversalCost(this, _parent);
        }
    }
}
