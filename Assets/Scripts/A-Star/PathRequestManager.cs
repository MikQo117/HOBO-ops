using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// </summary>
public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest        currentPathRequest;
    private Pathfinding        pathfinding;
    static PathRequestManager  instance;
    private bool               isProcessingPath;

    /// <summary>
    /// Requests a path to calculate.
    /// </summary>
    /// <param name="pathStart">Start point of the path.</param>
    /// <param name="pathEnd">End point of the path.</param>
    /// <param name="callback">Callback to the original function, see OnPathFound in TestUnit.cs.</param>
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool, int> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    
    /// <summary>
    /// Starts pathfind processing when there is no task ongoing.
    /// </summary>
    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.PathStart, currentPathRequest.PathEnd);
        }
    }

    /// <summary>
    /// Sends a callback to the path request and tries to process the next one.
    /// </summary>
    /// <param name="path">The found path.</param>
    /// <param name="success">Was the pathfind succesful?</param>
    public void FinishedProcessingPath(Vector2[] path, bool success, int pathLength)
    {
        currentPathRequest.Callback(path, success, pathLength);
        isProcessingPath = false;
        TryProcessNext();
    }

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    /// <summary>
    /// Stores the necessary data for a path request.
    /// </summary>
    private struct PathRequest
    {
        public Vector2 PathStart;
        public Vector2 PathEnd;
        public Action<Vector2[], bool, int> Callback;

        public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool, int> callback)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            Callback = callback;
        }
    }
}
