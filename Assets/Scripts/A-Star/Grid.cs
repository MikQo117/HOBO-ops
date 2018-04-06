using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask UnwalkableMask;
    public Vector2   GridWorldSize;
    public float     NodeRadius;
    private Node[,]  grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = NodeRadius * 2;
        //Convert world size to grid size
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        //Make new grid and calculate bottom left
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - (Vector2.right * GridWorldSize.x / 2) - (Vector2.up * GridWorldSize.y / 2);

        //For through all the grid points and create new nodes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Calculate world point for the node
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + NodeRadius) + Vector2.up * (y * nodeDiameter + NodeRadius);

                //Check for obstacles
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, NodeRadius, UnwalkableMask));

                //Create new node in current position
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition.y + GridWorldSize.y / 2) / GridWorldSize.y;

        //Make sure that given location is within the grid
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x,y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neigbhbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Middle node doesn't need a check
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //If the node is next to target and within bounds
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neigbhbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neigbhbours;
    }

    public List<Node> path;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, 1f));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if (n.walkable)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
#endif
}
