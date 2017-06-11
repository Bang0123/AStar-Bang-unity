using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public Transform Seeker, Target;
    private Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        FindAStarPath(Seeker.position, Target.position);
    }

    public void FindAStarPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.GetNodeFromWorldPos(startPos);
        Node targetNode = grid.GetNodeFromWorldPos(targetPos);
        List<Node> openNodes = new List<Node>();
        HashSet<Node> closedNodes = new HashSet<Node>();
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node node = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].F < node.F || openNodes[i].F == node.F)
                {
                    if (openNodes[i].H < node.H)
                        node = openNodes[i];
                }
            }
            openNodes.Remove(node);
            closedNodes.Add(node);
            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            foreach (var adjNode in grid.GetAdjacentLocations(node))
            {
                if (!adjNode.Walkable || closedNodes.Contains(adjNode))
                {
                    continue;
                }
                int newCostToNeighbour = node.G + GetTraversalCost(node, adjNode);
                if (newCostToNeighbour < adjNode.G || !openNodes.Contains(adjNode))
                {
                    adjNode.G = newCostToNeighbour;
                    adjNode.H = !adjNode.Expensive ? GetTraversalCost(adjNode, targetNode) : Mathf.RoundToInt((grid.GridWorldSize.x * grid.GridWorldSize.y)*.65f) + GetTraversalCost(adjNode, targetNode);
                    adjNode.Parent = node;

                    if (!openNodes.Contains(adjNode))
                        openNodes.Add(adjNode);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        grid.Path = path;

    }
    // Manhatten distance
    int GetTraversalCost(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

}
