using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcWaitState : NpcBaseState
{
    float timeTaken;
    public override void EnterState(NpcStateManager npc)
    {
        npc.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public override void UpdateState(NpcStateManager npc)
    {
        timeTaken += Time.deltaTime;
    }

    public override void OnCollisionEnter(NpcStateManager npc, Collision collision)
    {
      
    }
}
