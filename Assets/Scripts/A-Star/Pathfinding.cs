using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker;
    public Transform target;    

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position); 
    }

    private void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //List of nodes to evaluated
        List<Node> openSet = new List<Node>();

        //List of nodes have been evaluated
        HashSet<Node> closedSet = new HashSet<Node>();

        //Add starting node to open set
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {

            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    //Node with the lowest fCost
                    currentNode = openSet[i];
                }
            }

            //Remove from open set and move to closed set since it has been evaluated
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                //Path found
                RetracePath(startNode, targetNode);
                return;
            }

            //Check neighbours
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                //If in closed set (already evaluated) or untraversable then skip a iteration
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                //Calculate the neighbours new gCost
                int newMovementCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour);

                //If new path to neighbour is shorter (gCost is smaller) or it is not in the open set,
                //then update the costs of the node and set the parent for tracing the route back
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = getDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    //If neighbour not yet set to the open set, add it
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();

        //Tracing backwards 
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    private int getDistance(Node nodeA, Node nodeB)
    {
        int xDist = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int yDist = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (xDist > yDist)
        {
            return 14 * yDist + 10 * (xDist - yDist);
        }
        else
        {
            return 14 * xDist + 10 * (xDist - xDist);
        }

    }
}
