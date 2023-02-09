using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<WaypointSystem> RaceSystems;
    public List<WaypointSystem> ChallengeSystems;

    public void InitializeChallengeWPS(int index)
    {
        ChallengeSystems[index].Initialize();
    }

    public void InitializeRaceWPS(int index) 
    {
        RaceSystems[index].Initialize();
    }    

    public WaypointSystem GetRaceWPS(int index) 
    {
        return RaceSystems[index];
    }

    public WaypointSystem GetChallengeWPS(int index) 
    {
        return ChallengeSystems[index];
    }
}
