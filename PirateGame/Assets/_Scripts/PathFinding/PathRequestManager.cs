using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult> ();

    static PathRequestManager instance;
    PathFinding pathFinding;

    private void Awake ()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding> ();
    }

    private void Update ()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue ();
                    result.callback (result.path, result.succes);
                }
            }
        }
    }

    public static void RequestPath (PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathFinding.FindPath (request, instance.FinishProcessingPath);
        };
        threadStart.Invoke ();
    }

    public void FinishProcessingPath (PathResult result)
    {
        lock (results)
        {
            results.Enqueue (result);
        }
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool succes;
    public Action<Vector3[], bool> callback;

    public PathResult (Vector3[] path, bool succes, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.succes = succes;
        this.callback = callback;
    }
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest (Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}