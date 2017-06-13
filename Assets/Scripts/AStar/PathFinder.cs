using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        if (Input.GetButtonDown("Jump"))
        {
            FindAStarPath(Seeker.position, Target.position);
        }
    }

    public void FindAStarPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Node startNode = grid.GetNodeFromWorldPos(startPos);
        Node targetNode = grid.GetNodeFromWorldPos(targetPos);
        Heap<Node> openNodes = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedNodes = new HashSet<Node>();
        openNodes.Add(startNode);
        while (openNodes.Count > 0)
        {
            Node node = openNodes.RemoveFirst();
            closedNodes.Add(node);
            if (node == targetNode)
            {
                stopwatch.Stop();
                print("Path found in " + stopwatch.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);
                return;
            }
            foreach (var adjNode in grid.GetAdjacentLocations(node))
            {
                if (!adjNode.Walkable || closedNodes.Contains(adjNode))
                    continue;
                int newCostToNeighbour = node.G + GetTraversalCost(node, adjNode);
                if (newCostToNeighbour < adjNode.G || !openNodes.Contains(adjNode))
                {
                    adjNode.G = newCostToNeighbour;
                    adjNode.H = !adjNode.Expensive ? GetTraversalCost(adjNode, targetNode) :
                        Mathf.RoundToInt(grid.MaxSize * .65f)
                        + GetTraversalCost(adjNode, targetNode);
                    adjNode.Parent = node;

                    if (!openNodes.Contains(adjNode))
                        openNodes.Add(adjNode);
                    else
                        openNodes.UpdateItem(adjNode);
                    
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
