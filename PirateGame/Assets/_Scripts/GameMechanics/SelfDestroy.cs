using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime;
    void Start()
    {
        GameObject.Destroy(gameObject, destroyTime);
    }
}
