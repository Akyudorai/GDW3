using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [HideInInspector] public WaypointSystem system;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            system.NextWaypoint();
        }
    }
}
