using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public LayerMask mask;
    public Transform target;
    Camera cam;
    public bool onlyOnDoubleClick;

    public void Start()
    {
        cam = Camera.main;
        target = GetComponent<Transform>();
        useGUILayout = false;
    }

    public void OnGUI()
    {
        if (onlyOnDoubleClick && cam != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
        {
            UpdateTargetPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!onlyOnDoubleClick && cam != null)
        {
            UpdateTargetPosition();
        }
    }

    public void UpdateTargetPosition()
    {
        Vector3 newPosition = Vector3.zero;
        bool positionFound = false;

        //Fire a ray through the scene at the mouse position and place the target where it hits
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask))
        {
            newPosition = hit.point;
            positionFound = true;
        }

        if (positionFound && newPosition != target.position)
        {
            target.position = newPosition;
        }
    }
}
