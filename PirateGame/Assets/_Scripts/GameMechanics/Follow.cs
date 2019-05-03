using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject objectToFollow;
    [SerializeField]
    public float offsetX, offsetY;

    private void Update()
    {
        transform.position = new Vector3(objectToFollow.transform.position.x + offsetX, objectToFollow.transform.position.y + offsetY, 0);
    }
}
