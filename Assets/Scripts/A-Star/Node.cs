using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Vector2 WorldPosition;
    public bool    Walkable;

    public int     GridX;
    public int     GridY;

    //Distance from starting node
    public int     GCost;

    //(heuristic) Distance from end node
    public int     HCost;

    public Node    Parent;
    private int    heapIndex;

    /// <summary>
    /// Returns gCost + hCost.
    /// </summary>
    public int FCost
    {
        get
        {
            ///<value>Gets the value of the fCost</value>
            return GCost + HCost;
        }
    }

    /// <summary>
    /// Nodes index in the heap.
    /// </summary>
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    /// <summary>
    /// Node constructor.
    /// </summary>
    /// <param name="walkable">Is this node walkable?</param>
    /// <param name="worldPosition">Nodes position in world space.</param>
    /// <param name="gridX">Nodes gridX position.</param>
    /// <param name="gridY">Nodes gridY position.</param>
    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    /// <summary>
    /// Compares the nodes fCost, if they're equal, compares hCost instead.
    /// </summary>
    /// <param name="nodeToCompare"></param>
    /// <returns>Returns 1 if this nodes fCost or hCost is lower than the parameter node, else returns -1.</returns>
    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        //Integer.CompareTo() returns 1 if the integer is higher
        //We want to return 1 if the hCost is lower 
        return -compare;
    }
}
