using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool Walkable;
    public int G, H;
    public int GridX, GridY;
    public Vector3 WorldPosition;
    public bool Expensive;
    public int HeapIndex { get; set; }
    public Node Parent { get; set; }
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
    public int CompareTo(Node other)
    {
        int compare = F.CompareTo(other.F);
        if (compare == 0)
        {
            compare = H.CompareTo(other.H);
        }
        return -compare;
    }
}
