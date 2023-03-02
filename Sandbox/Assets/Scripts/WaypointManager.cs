using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<WaypointSystem> RaceSystems;

    public void InitializeRaceWPS(int index) 
    {
        RaceSystems[index].Initialize();
    }    

    public WaypointSystem GetRaceWPS(int index) 
    {
        return RaceSystems[index];
    }
}
