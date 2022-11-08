using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCompleteState : NpcBaseState
{
    public override void EnterState(NpcStateManager npc)
    {
        Debug.Log("Entered completion state.");
        npc.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    public override void UpdateState(NpcStateManager npc)
    {
        
    }

    public override void OnCollisionEnter(NpcStateManager npc, Collision collision)
    {

    }
}
