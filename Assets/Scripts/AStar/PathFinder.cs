using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public Transform Seeker, Target;
    private PathManager pathManager;
    private Grid grid;

    void Awake()
    {
        pathManager = GetComponent<PathManager>();
        grid = GetComponent<Grid>();
    }

    //void Update()
    //{
    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        FindAStarPath(Seeker.position, Target.position);
    //    }
    //}

    public void StartFindAStarPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindAStarPath(startPos, targetPos));
    }

    public IEnumerator FindAStarPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.GetNodeFromWorldPos(startPos);
        Node targetNode = grid.GetNodeFromWorldPos(targetPos);
        if (startNode.Walkable && targetNode.Walkable)
        {
            Heap<Node> openNodes = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedNodes = new HashSet<Node>();
            openNodes.Add(startNode);
            while (openNodes.Count > 0)
            {
                Node node = openNodes.RemoveFirst();
                closedNodes.Add(node);
                if (node == targetNode)
                {
                    pathSuccess = true;
                    break;
                }
                foreach (var adjNode in grid.GetAdjacentLocations(node))
                {
                    if (!adjNode.Walkable || closedNodes.Contains(adjNode))
                        continue;
                    int newCostToNeighbour = node.G + GetTraversalCost(node, adjNode);
                    if (newCostToNeighbour < adjNode.G || !openNodes.Contains(adjNode))
                    {
                        adjNode.G = newCostToNeighbour;
                        adjNode.H = !adjNode.Expensive
                            ? GetTraversalCost(adjNode, targetNode)
                            : Mathf.RoundToInt(grid.MaxSize * .65f)
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
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        pathManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
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
