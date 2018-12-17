using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridForA : MonoBehaviour {

    public LayerMask unwalkableMask;
    public Vector2 gridWordlSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWordlSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWordlSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldButtomLeft = transform.position - Vector3.right * gridWordlSize.x / 2 - Vector3.up * gridWordlSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldButtomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    public Node NodeFromeWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWordlSize.x / 2) / gridWordlSize.x;
        float percentY = (worldPosition.y + gridWordlSize.y / 2) / gridWordlSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];

    }

    void OnDrawGizmos()
    {
        if(grid!=null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
