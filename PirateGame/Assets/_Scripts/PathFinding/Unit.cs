using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Transform target;
    public float speed = 5;
    public float turnDst = 2.5f;
    public float turnSpeed = 0.5f;

    Path path;
    Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()// change this Start() with Update() or anything else when the tutorial has been done
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] waipoints, bool pathSuccesful)
    {
        if(pathSuccesful)
        {
            path = new Path(waipoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        //this line of code are for 3d, i wil comment them and add the 2d ones after ep 10
        bool followingPath = true;
        int pathIndex = 0;
        RotatingThePlayer(path.lookPoints[pathIndex]);
        Debug.Log(followingPath);

        while (followingPath)
        {
            Debug.Log("is in following path");
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            Debug.Log(pathIndex);
            if(path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                Debug.Log(path.finishLineIndex);
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                RotatingThePlayer(path.lookPoints[pathIndex]);
                //transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path!=null)
        {
            path.DrawWithGizmos();
        }
    }

    protected virtual void RotatingThePlayer(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        float angleDifferenceToTarget = Vector2.SignedAngle(transform.up, direction);

        //Flattens difference to -1f/1f * delta * turn rate
        float rotationChange = Mathf.Sign(angleDifferenceToTarget) * Time.deltaTime * turnSpeed;

        //Todo: Cleanup/rework this temporary shake mitigation code
        /*if (Mathf.Abs(rotationChange) > Mathf.Abs(angleDifferenceToTarget)) rotationChange = angleDifferenceToTarget;
        if ((Mathf.Abs(angleDifferenceToTarget) - Mathf.Abs(rotationChange)).IsWithinRange(0, .01f)) rotationChange = 0;*/

        //Apply rotation
        Vector3 localEulerAngles = transform.localEulerAngles;
        localEulerAngles.z += rotationChange;
        //localEulerAngles.z = Mathf.Clamp(localEulerAngles.z.TauToPi() + rotationChange, -weapon.maxAngle, weapon.maxAngle);
        localEulerAngles.Set(0, 0, localEulerAngles.z);
        transform.localEulerAngles = localEulerAngles;
    }
}
