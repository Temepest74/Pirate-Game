using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{

    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;

    public Path (Vector3[] waipoints, Vector3 startPos, float turnDst, float stoppingDst)
    {
        lookPoints = waipoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 previsousPoint = V3ToV2 (startPos);

        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2 (lookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previsousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
            turnBoundaries[i] = new Line (turnBoundaryPoint, previsousPoint - dirToCurrentPoint * turnDst);
            previsousPoint = turnBoundaryPoint;
        }

        float dstFromEndPoint = 0;
        for (int i = lookPoints.Length - 1; i > 0; i--)
        {
            dstFromEndPoint += Vector3.Distance (lookPoints[i], lookPoints[i - 1]);
            if (dstFromEndPoint > stoppingDst)
            {
                slowDownIndex = i;
                break;
            }
        }
    }

    Vector2 V3ToV2 (Vector3 v)
    {
        return new Vector2 (v.x, v.y);
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;

        foreach(Vector3 p in lookPoints)
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach (Line l in turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}