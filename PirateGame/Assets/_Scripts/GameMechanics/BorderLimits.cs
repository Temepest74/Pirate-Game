using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderLimits : MonoBehaviour
{

    PolygonCollider2D border;

    private void Start()
    {
        border = GameObject.Find("Border").GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        float[ ] borderPointsX = new float[border.points.Length];
        float[ ] borderPointsY = new float[border.points.Length];
        for (int i = 0; i < border.points.Length; i++)
        {
            borderPointsX[i] = border.points[i].x;
            borderPointsY[i] = border.points[i].y;
        }
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, Mathf.Min(borderPointsX), Mathf.Max(borderPointsX)),
            Mathf.Clamp(transform.position.y, Mathf.Min(borderPointsY), Mathf.Max(borderPointsY)),
            transform.position.z
        );
    }
}