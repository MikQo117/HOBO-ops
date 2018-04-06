using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 worldPosition;
    public bool    walkable;

    public Node(bool walkable, Vector2 worldPosition)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
    }
}
