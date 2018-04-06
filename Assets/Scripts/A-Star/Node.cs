using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 worldPosition;
    public bool    walkable;

    public int gridX, gridY;

    //Distance from starting node
    public int gCost;

    //(heuristic) Distance from end node
    public int hCost;

    public Node parent;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
