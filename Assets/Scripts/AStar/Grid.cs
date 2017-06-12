using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform PlayerTransform;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public LayerMask UnwalkableMask;
    public LayerMask ExpensiveMask;
    public List<Node> Path;
    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;


    void Awake()
    {
        nodeDiameter = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        CreateGrid();

    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 -
                                  Vector3.forward * GridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) 
                    + Vector3.forward * (y * nodeDiameter + NodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableMask);
                bool expensive = Physics.CheckSphere(worldPoint, NodeRadius, ExpensiveMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y , expensive);
            }
        }
    }

    public List<Node> GetAdjacentLocations(Node node)
    {
        List<Node> adjecents = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.GridX + x;
                int checkY = node.GridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    adjecents.Add(grid[checkX, checkY]);
            }
        }
        return adjecents;
    }

    public Node GetNodeFromWorldPos(Vector3 worldpos)
    {
        float percentX = Mathf.Clamp01(worldpos.x / GridWorldSize.x + .5f);
        float percentY = Mathf.Clamp01(worldpos.z / GridWorldSize.y + .5f);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
        if (grid != null)
        {
            Node playerNode = GetNodeFromWorldPos(PlayerTransform.position);
            foreach (var node in grid)
            {
                Gizmos.color = node.Walkable ? Color.green : Color.red;
                if (node.Expensive)
                    Gizmos.color = Color.yellow;
                if (playerNode == node)
                    Gizmos.color = Color.black;
                if (Path != null)
                    if (Path.Contains(node))
                        Gizmos.color = Color.blue;
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
