using System.Collections; using System.Collections.Generic; using UnityEngine; using System;  public class PathFinding : MonoBehaviour
{     PathRequestManager requestManager;      GridForA grid;      private void Awake()     {         requestManager = GetComponent<PathRequestManager>();         grid = GetComponent<GridForA>();     }      public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }      IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)     {          Vector3[] waypoints = new Vector3[0];         bool pathSucces = false;          Node startNode = grid.NodeFromWorldPoint(startPos);         Node targetNode = grid.NodeFromWorldPoint(targetPos);          if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSucces = true;
                    break;
                }

                foreach (Node neighbourd in grid.GetNeighbourds(currentNode))
                {
                    if (!(neighbourd.walkable) || closedSet.Contains(neighbourd))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbourd) + neighbourd.movementPenalty;
                    if (newMovementCostToNeighbour < neighbourd.gCost || !openSet.Contains(neighbourd))
                    {
                        neighbourd.gCost = newMovementCostToNeighbour;
                        neighbourd.hCost = GetDistance(neighbourd, targetNode);

                        neighbourd.parent = currentNode;

                        if (!openSet.Contains(neighbourd))
                        {
                            openSet.Add(neighbourd);
                        }else
                        openSet.UpdateItem(neighbourd);
                    }
                }
            }         }         yield return null;         if(pathSucces)
        {
            waypoints = RetracePath(startNode, targetNode);
        }         requestManager.FinishProcessingPath(waypoints, pathSucces);     }      Vector3[] RetracePath(Node startNode, Node endNode)     {         List<Node> path = new List<Node>();         Node currentNode = endNode;          while (currentNode != startNode)         {             path.Add(currentNode);             currentNode = currentNode.parent;         }         path.Add(startNode);         Vector3[] waypoints = SimplifyPath(path);         Array.Reverse(waypoints);         return waypoints;     }      Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();         Vector2 directionOld = Vector2.zero;          for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);             if(directionNew != directionOld)
            {
                waypoints.Add(path[i-1].worldPosition);
            }             directionOld = directionNew;
        }         return waypoints.ToArray();
    }      int GetDistance(Node nodeA, Node nodeB)     {         int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);         int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);          if (dstX > dstY)         {             return 14 * dstY + 10 * (dstX - dstY);         }         else         {             return 14 * dstX + 10 * (dstY - dstX);         }     } }  