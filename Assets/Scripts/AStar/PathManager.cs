using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{

    private Queue<PathRequest> _requestQueue = new Queue<PathRequest>();
    private PathRequest _currentRequest;

    private static PathManager _instance;
    private PathFinder _pathfinding;

    private bool _isProcessing;

    void Awake()
    {
        _instance = this;
        _pathfinding = GetComponent<PathFinder>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        _instance._requestQueue.Enqueue(newRequest);
        _instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!_isProcessing && _requestQueue.Count > 0)
        {
            _currentRequest = _requestQueue.Dequeue();
            _isProcessing = true;
            _pathfinding.StartFindAStarPath(_currentRequest.PathStart, _currentRequest.PathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        _currentRequest.callback(path, success);
        _isProcessing = false;
        TryProcessNext();
    }
}
public struct PathRequest
{
    public Vector3 PathStart;
    public Vector3 PathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        PathStart = _start;
        PathEnd = _end;
        callback = _callback;
    }

}
