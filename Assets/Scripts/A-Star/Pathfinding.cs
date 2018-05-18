using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using UnityEngine;

/// <summary>
/// Class that finds paths in a grid based environment.
/// </summary>
public class Pathfinding : MonoBehaviour
{
    private PathRequestManager requestManager;
    private Grid               grid;
    private int                pathLength = 0;


    /// <summary>
    /// Finds a path between A and B using the A* algorithm.
    /// </summary>
    /// <param name="startPos">Start position in world coordinates.</param>
    /// <param name="targetPos">End position in world coordinates.</param>
    private IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        //Temporary perfomance monitoring
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //Only find path if the start and target are walkable
        if (startNode.Walkable && targetNode.Walkable)
        {
            //List of nodes to evaluated //List of nodes have been evaluated
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            //Add starting node to open set
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {

                Node currentNode = openSet.RemoveFirst();


                /*for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        //Node with the lowest fCost
                        currentNode = openSet[i];
                    }
                }

                //Remove from open set and move to closed set since it has been evaluated
                openSet.Remove(currentNode);*/
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //Path found
                    sw.Stop();
                    //print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                //Check neighbours
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    //If in closed set (already evaluated) or untraversable then skip a iteration
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    //Calculate the neighbours new gCost
                    int newMovementCostToNeighbour = currentNode.GCost + getDistance(currentNode, neighbour);

                    //If new path to neighbour is shorter (gCost is smaller) or it is not in the open set,
                    //then update the costs of the node and set the parent for tracing the route back
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = getDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;
                        //If neighbour not yet set to the open set, add it
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            } 
        }
        yield return null;
        if (pathSuccess)
        {
            //Finalize the waypoint array
            waypoints = RetracePath(startNode, targetNode);
            foreach (Vector2 item in waypoints)
            {
                //print("waypoints: " + item); 
            }
            //Pathfind is only successful when there's a node to go to
            pathSuccess = waypoints.Length > 0;
        }
        //Return to request manager
        requestManager.FinishedProcessingPath(waypoints, pathSuccess, pathLength);
    }

    /// <summary>
    /// Retraces the path back through parents, and reverses it to match the actual route.
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    private Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        //Tracing backwards 
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Add(startNode);
        pathLength = path.Count;
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints); //*anteeksimitävittua*
        return waypoints;
    }

    /// <summary>
    /// Simplifies the path to the necessary nodes only.
    /// </summary>
    /// <param name="path">Path to simplify.</param>
    /// <returns>Simplified path.</returns>
    private Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        /*for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i-1].WorldPosition);
            }
            directionOld = directionNew;
        }*/

        for (int i = 0; i < path.Count; i++)
        {
            waypoints.Add(path[i].WorldPosition);
        }
        return waypoints.ToArray();
    }

    /// <summary>
    /// Calculates the cost between two nodes.
    /// </summary>
    /// <param name="nodeA">Start node.</param>
    /// <param name="nodeB">End node.</param>
    /// <returns>Returns the calculated cost.</returns>
    private int getDistance(Node nodeA, Node nodeB)
    {
        int xDist = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int yDist = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (xDist > yDist)
        {
            return 14 * yDist + 10 * (xDist - yDist);
        }
        else
        {
            return 14 * xDist + 10 * (yDist - xDist);
        }
    }

    /// <summary>
    /// Public function to start pathfinding.
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }
}
