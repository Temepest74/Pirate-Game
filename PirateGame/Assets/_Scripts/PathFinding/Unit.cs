using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveTreshhold = .4f;

    public Transform target;
    public float speed = 5;
    public float turnDst = 2.5f;
    public float turnSpeed = 0.5f;
    public float stoppingDistance = 10;
    public float noMoreMovementDistance;

    Path path;
    Rigidbody2D rb2D;

    [HideInInspector]
    public float angleDifferenceToTarget;
    public float angleDifferenceToTargetTreshhold;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //set target
        StartCoroutine(UpdatePath());
    }

    public void OnPathFound(Vector3[] waipoints, bool pathSuccesful)
    {
        if(pathSuccesful)
        {
            path = new Path(waipoints, transform.position, turnDst, stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if(Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
        float sqrtMoveTreshhold = pathUpdateMoveTreshhold * pathUpdateMoveTreshhold;
        Vector3 targetPosOld = target.position;

        while(true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrtMoveTreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        RotatingThePlayer(path.lookPoints[pathIndex]);

        float speedPercent = 1f;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            while(path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                if (pathIndex >= path.slowDownIndex && stoppingDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDistance);
                    if(speedPercent < .01f)
                    {
                        followingPath = false;
                    }
                }
                RotatingThePlayer(path.lookPoints[pathIndex]);
                if (Mathf.Abs(angleDifferenceToTarget) < angleDifferenceToTargetTreshhold && (target.position - transform.position).sqrMagnitude > noMoreMovementDistance * noMoreMovementDistance)
                {
                    transform.Translate(Vector3.up * speed * Time.deltaTime * speedPercent, Space.Self);//change it when ballancing the game
                }
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
        angleDifferenceToTarget = Vector2.SignedAngle(transform.up, direction);
        //Flattens difference to -1f/1f * delta * turn rate
        float rotationChange = Mathf.Sign(angleDifferenceToTarget) * Time.deltaTime * turnSpeed;

        //Apply rotation
        Vector3 localEulerAngles = transform.localEulerAngles;
        localEulerAngles.z += rotationChange;
        localEulerAngles.Set(0, 0, localEulerAngles.z);
        transform.localEulerAngles = localEulerAngles;
    }
}
