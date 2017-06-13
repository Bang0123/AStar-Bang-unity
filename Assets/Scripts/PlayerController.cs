using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform Target;
    private Rigidbody _rb;
    public float Speed = 2;
    private Vector3[] path;
    private int TargetIndex;

    void Start()
    {
        PathManager.RequestPath(transform.position, Target.position, OnPathFound);
    }

    // Finder hele tiden en path dog har fejl tilsidst :p
    //void FixedUpdate()
    //{
    //    PathManager.RequestPath(transform.position, Target.position, OnPathFound);
    //}

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            TargetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                TargetIndex++;
                if (TargetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[TargetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = TargetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);
                if (i == TargetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}