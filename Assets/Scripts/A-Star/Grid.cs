using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles pathfinding grid
/// </summary>
public class Grid : MonoBehaviour
{
    public bool       DisplayGridGizmos;
    public LayerMask  UnwalkableMask;
    public Vector2    GridWorldSize;
    public float      NodeRadius;
    public List<Node> Path;
    private Node[,]   grid;

    private float     nodeDiameter;
    private int       gridSizeX, gridSizeY;

    /// <summary>
    /// Creates a grid for the pathfinding algorithm.
    /// </summary>
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

    /// <summary>
    /// Returns a node corresponding to the given world position.
    /// </summary>
    /// <param name="worldPosition">Point in world space.</param>
    /// <returns>Corresponding node.</returns>
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

    /// <summary>
    /// Gets the neighbour of the given node.
    /// </summary>
    /// <param name="node">Node to evaluate.</param>
    /// <returns>List of neighbour nodes.</returns>
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neigbhbours = new List<Node>();

        //Go through the neighboring nodes
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Middle node doesn't need a check
                if (x == 0 && y == 0)
                {
                    continue;
                }

                //Get the nodes grid position
                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                //If the node is next to target and within bounds
                if ((checkX >= 0 && checkX < gridSizeX) && (checkY >= 0 && checkY < gridSizeY))
                {
                    neigbhbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neigbhbours;
    }

    /// <summary>
    /// Returns the grids maximum possible size.
    /// </summary>
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    /// <summary>
    /// Calculates grid size and creates it.
    /// </summary>
    private void Awake()
    {
        nodeDiameter = NodeRadius * 2;
        //Convert world size to grid size
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

#if UNITY_EDITOR
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, 1f));

        if (grid != null && DisplayGridGizmos)
        {
            foreach (Node n in grid)
            {
                if (n.Walkable)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        } 
    }*/

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, 1));

        if (true)
        {
            if (grid != null && DisplayGridGizmos)
            {
                /*foreach (Node n in Path)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - .1f));
                }*/

                foreach (Node n in grid)
                {
                    if(n.Walkable)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - .01f));
                }
            }
        }
        else
        {

            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                    if (Path != null)
                        if (Path.Contains(n))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }

#endif
}
