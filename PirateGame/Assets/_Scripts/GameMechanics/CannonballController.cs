using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballController : MonoBehaviour
{
    public float speed;
    private void FixedUpdate()
    {
        gameObject.transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
