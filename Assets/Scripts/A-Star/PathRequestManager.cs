using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{

    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;

    static PathRequestManager instance;
    private Pathfinding pathfinding;

    private bool isProcessingPath;

    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();

    }
    
    struct PathRequest
    {
        public Vector2 PathStart;
        public Vector2 PathEnd;
        public Action<Vector2[], bool> Callback;

        public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            Callback = callback;
        }
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.PathStart, currentPathRequest.PathEnd);
        }
    }

    public void FinishedProcessingPath(Vector2[] path, bool success)
    {
        currentPathRequest.Callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
}
