using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHelpState : NpcBaseState
{
    public bool helpFound = false;
    //float currentTime = 0f;
    //float targetTime = 3f;
    public override void EnterState (NpcStateManager npc)
    {
        //Debug.Log(npc.gameObject.name);
        
    }

    public override void UpdateState (NpcStateManager npc)
    {
       
    }

    public override void OnCollisionEnter (NpcStateManager npc, Collision collision)
    {

    }
}
