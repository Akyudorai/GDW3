using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    public int SystemID;
    public List<Waypoint> Waypoints = new List<Waypoint>();
    public int WaypointIndex = 0;

    private void Start() 
    {
        foreach (Waypoint wp in Waypoints) 
        {
            wp.system = this;
            wp.gameObject.SetActive(false);
        }
    }

    public void Initialize() 
    {        
        SetIndex(0);
    }

    public void SetIndex(int index) 
    {
        WaypointIndex = index;
        Waypoints[WaypointIndex].gameObject.SetActive(true);        
    }

    public void NextWaypoint() 
    {        
        Waypoints[WaypointIndex].gameObject.SetActive(false);
     
        if (WaypointIndex == Waypoints.Count-1) {
            Debug.Log("Waypoint System Complete!");
            EventManager.OnRaceEnd?.Invoke(false);
        } else 
        {
            SetIndex(WaypointIndex+1);
        }
        
    }

}
