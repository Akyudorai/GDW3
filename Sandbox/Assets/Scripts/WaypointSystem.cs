using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    public int SystemID;
    public Transform Beginning, End;
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

        if (WaypointIndex < Waypoints.Count-1) {
            Waypoints[WaypointIndex].ActivatePointer(Waypoints[WaypointIndex+1].PointerObj);
        }     
    }

    public void NextWaypoint() 
    {        
        Waypoints[WaypointIndex].gameObject.SetActive(false);
        Waypoints[WaypointIndex].PointerObj.SetActive(false);
     
        if (WaypointIndex == Waypoints.Count-1) {
            Debug.Log("Waypoint System Complete!");

            if (RaceManager.GetInstance().m_RaceActive) EventManager.OnRaceEnd?.Invoke(false);
            else if (RaceManager.GetInstance().m_ChallengeActive) EventManager.OnChallengeEnd?.Invoke(false);            
        } else 
        {
            SetIndex(WaypointIndex+1);
        }
        
    }

}
