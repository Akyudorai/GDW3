using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcWaitState : NpcBaseState
{
    
    public override void EnterState(NpcStateManager npc)
    {
        npc.GetComponentInChildren<Renderer>().material.color = Color.yellow;
    }

    public override void UpdateState(NpcStateManager npc)
    {
        
    }

    public override void OnCollisionEnter(NpcStateManager npc, Collision collision)
    {
      
    }
}
