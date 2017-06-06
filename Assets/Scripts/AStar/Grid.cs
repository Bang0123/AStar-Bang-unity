using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform PlayerTransform;
    private Node[,] grid;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public LayerMask UnwalkableMask;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;


    void Start()
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
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.forward * (y * nodeDiameter + NodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    public Node NodeFromWorldPos(Vector3 worldpos)
    {
        //float percentX = Mathf.Clamp01(worldpos.x /GridWorldSize.x + .5f);
        //float percentY = Mathf.Clamp01(worldpos.z / GridWorldSize.y +.5f);
        float percentX = (worldpos.x - transform.position.x) / GridWorldSize.x + 0.5f - (NodeRadius / GridWorldSize.x);
        float percentY = (worldpos.z - transform.position.z) / GridWorldSize.y + 0.5f - (NodeRadius / GridWorldSize.y);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
        if (grid != null)
        {
            Node playerNode = NodeFromWorldPos(PlayerTransform.position);
            foreach (var node in grid)
            {
                Gizmos.color = node.Walkable ? Color.green : Color.red;

                if (playerNode == node)
                {
                    Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

}
