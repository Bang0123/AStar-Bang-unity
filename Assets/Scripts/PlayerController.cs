using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IObservers
{
    public Transform Target;
    private Rigidbody _rb;
    public float Speed = 2.5f;
    private Vector3[] path;
    private int TargetIndex;
    private TargetMover gobj;

    void Start()
    {
        gobj = GameObject.FindGameObjectWithTag("Target").GetComponent<TargetMover>();
        if (gobj != null)
        {
            gobj.registerObserver(this);
            PathManager.RequestPath(transform.position, Target.position, OnPathFound);
        }
    }

    void OnDestroy()
    {
        if (gobj != null)
        {
            gobj.removeObserver(this);
        }
    }

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
                Gizmos.color = Color.blue;
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

    public void OnNotify()
    {
        PathManager.RequestPath(transform.position, Target.position, OnPathFound);
    }
}