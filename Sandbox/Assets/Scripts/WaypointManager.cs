using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<WaypointSystem> Systems;

    public void Initialize(int index) 
    {
        Systems[index].Initialize();
    }    
}
