using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public LayerMask layer;
    Transform transformOld;

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveTreshhold = .4f;

    public Transform target;
    public float speed = 5;
    public float turnDst = 2.5f;
    public float turnSpeed = 0.5f;
    public float stoppingDistance = 10;
    public float noMoreMovementDistance;
    public float targetCheckSizeX;
    public float targetCheckSizeY;

    Path path;
    GridForA grid;

    [HideInInspector]
    public float angleDifferenceToTarget;
    public float angleDifferenceToTargetTreshhold;

    private bool setRandomTarget;

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("PathfindingHolder").GetComponent<GridForA>();
    }

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        StartCoroutine("SetTarget");
        StartCoroutine("UpdatePath");
    }

    private void Update()
    {

        FindNearestTarget();
    }

    public void OnPathFound(Vector3[] waipoints, bool pathSuccesful)
    {
        //check this for stopping path
        if (pathSuccesful && target != null && gameObject.activeSelf)
        {
            path = new Path(waipoints, transform.position, turnDst, stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
        float sqrtMoveTreshhold = pathUpdateMoveTreshhold * pathUpdateMoveTreshhold;
        Vector3 targetPosOld = target.position;
        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if (target != null)
                if ((target.position - targetPosOld).sqrMagnitude > sqrtMoveTreshhold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                    targetPosOld = target.position;
                }
        }
    }

    IEnumerator FollowPath()
    {
        //check
        if (!gameObject.GetComponent<IEntityData>().GetEntityData().isDead || gameObject.GetComponent<IEntityData>() == null)
        {
            bool followingPath = true;
            int pathIndex = 0;

            RotatingThePlayer(path.lookPoints[0]);

            float speedPercent = 1f;
            while (followingPath && !gameObject.GetComponent<IEntityData>().GetEntityData().isDead || gameObject.GetComponent<IEntityData>() == null)
            {
                Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
                while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                {
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;
                        break;
                    }
                    else
                    {
                        pathIndex++;
                    }
                }

                if (followingPath)
                {
                    if (pathIndex >= path.slowDownIndex && stoppingDistance > 0)
                    {
                        speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDistance);
                        if (speedPercent < .01f)
                        {
                            followingPath = false;
                        }
                    }
                    RotatingThePlayer(path.lookPoints[pathIndex]);
                    if (transform != null && target != null)
                    {
                        if (Mathf.Abs(angleDifferenceToTarget) < angleDifferenceToTargetTreshhold && (target.position - transform.position).sqrMagnitude > noMoreMovementDistance * noMoreMovementDistance)
                        {
                            transform.Translate(Vector3.up * speed * Time.deltaTime * speedPercent, Space.Self);//change it when ballancing the game
                        }
                    }
                }
                yield return null;
            }
        }
    }

    /*public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }*/

    protected virtual void RotatingThePlayer(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        angleDifferenceToTarget = Vector2.SignedAngle(transform.up, direction);
        //Flattens difference to -1f/1f * delta * turn rate
        if (angleDifferenceToTarget < -1 || angleDifferenceToTarget > 1)
        {
            float rotationChange = Mathf.Sign(angleDifferenceToTarget) * Time.deltaTime * turnSpeed;
            //Apply rotation
            Vector3 localEulerAngles = transform.localEulerAngles;
            localEulerAngles.z += rotationChange;
            localEulerAngles.Set(0, 0, localEulerAngles.z);
            transform.localEulerAngles = localEulerAngles;
        }
    }

    private void FindNearestTarget()
    {
        RaycastHit2D[] raycastHit2Ds = Physics2D.BoxCastAll(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(targetCheckSizeX, targetCheckSizeY),
            0,
            Vector2.zero,
            Mathf.Infinity,
            layer.value
            );
        int notUsable = 0;
        foreach (RaycastHit2D item in raycastHit2Ds)
        {
            if(item.collider.gameObject.GetComponent<IEntityData>().GetEntityData().isDead ||
                item.collider.gameObject == gameObject)
            {
                notUsable++;
            }
        }
        if (raycastHit2Ds.Length > 1 && raycastHit2Ds.Length != notUsable)
        {
            StopCoroutine("SetTarget");
            float minDist = Mathf.Infinity;
            foreach (RaycastHit2D raycastHit in raycastHit2Ds)
            {
                if (target != null && target.gameObject.GetComponent<IEntityData>() == null)
                {
                    target.gameObject.GetComponent<SelfDestroy>().DestroyNow();
                }
                if (raycastHit.collider.gameObject != gameObject &&
                    raycastHit.collider.gameObject.GetComponent<IEntityData>() != null &&
                    raycastHit.collider.gameObject.GetComponent<IEntityData>().GetEntityData().isDead == false)
                {
                    float dist = Mathf.Abs(Vector3.Distance(raycastHit.collider.gameObject.transform.position, transform.position));
                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = raycastHit.collider.gameObject.transform;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        if (target == null)
        {
            StartCoroutine("SetTarget");
            return;
        }
        if (target?.gameObject.GetComponent<IEntityData>()?.GetEntityData().isDead == true)
        {
            StartCoroutine("SetTarget");
            return;
        }
        if(target?.gameObject.GetComponent<IEntityData>() != null)
        {
           if(Mathf.Abs( Vector3.Distance(transform.position, target.position))> 7)
            {
                StartCoroutine("SetTarget");
            }
        }
    }

    private IEnumerator SetTarget()
    {
        while (true)
        {
            transformOld = transform;
            if (target != null && target.gameObject.GetComponent<IEntityData>() == null)
            {
                target.gameObject.GetComponent<SelfDestroy>().DestroyNow();
            }
            target = new GameObject().transform;
            target.name = string.Concat(gameObject.name, "target");
            target.gameObject.AddComponent<SelfDestroy>();
            target.gameObject.GetComponent<SelfDestroy>().onStartDestroy = false;
            target.gameObject.GetComponent<SelfDestroy>().destroyTime = 0;
            if (transformOld.position == transform.position)
            {
                int x;
                int y;
                while (true)
                {
                    x = (int)Random.Range(0, grid.gridSizeX - 1);
                    y = (int)Random.Range(0, grid.gridSizeY - 1);
                    if (grid.grid[x, y].walkable)
                    {
                        break;
                    }
                }
                target.position = grid.grid[x, y].worldPosition;
                RotatingThePlayer(target.position);
            }
            transformOld.position = transform.position;
            yield return new WaitForSeconds(5);
        }
    }

    private float GetDistance(GameObject obj)
    {
        return Mathf.Abs((transform.position - obj.transform.position).sqrMagnitude);
    }
}

