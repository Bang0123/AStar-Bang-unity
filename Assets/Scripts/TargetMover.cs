using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour, IObservable
{
    public LayerMask Mask;
    public Transform Target;
    public bool OnlyOnDoubleClick;
    private Camera _cam;
    private List<IObservers> observers = new List<IObservers>();

    public void Start()
    {
        _cam = Camera.main;
        Target = GetComponent<Transform>();
        useGUILayout = false;
    }

    public void OnGUI()
    {
        if (OnlyOnDoubleClick && _cam != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
        {
            UpdateTargetPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnlyOnDoubleClick && _cam != null)
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
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, Mask))
        {
            newPosition = hit.point;
            positionFound = true;
        }

        if (positionFound && newPosition != Target.position)
        {
            Target.position = newPosition;
        }
        notifyObservers();
    }
    
    public void registerObserver(IObservers o)
    {
        observers.Add(o);
    }

    public void removeObserver(IObservers o)
    {
        observers.Remove(o);
    }

    public void notifyObservers()
    {
        if (observers.Count > 0)
        {
            foreach (var item in observers)
            {
                item.OnNotify();
            };
        }
    }
}
public interface IObservable
{
    void registerObserver(IObservers o);
    void removeObserver(IObservers o);
    void notifyObservers();
}
public interface IObservers
{
    void OnNotify();
}
