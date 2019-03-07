using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime = 3f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    public void DestroyNow()
    {
        Destroy(gameObject);
    }
}
