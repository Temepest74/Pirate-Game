using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime = 3f;
    public bool onStartDestroy = true;
    void Start()
    {
        if (onStartDestroy)
            Destroy(gameObject, destroyTime);
    }
    public void DestroyNow()
    {
        Destroy(gameObject);
    }
}