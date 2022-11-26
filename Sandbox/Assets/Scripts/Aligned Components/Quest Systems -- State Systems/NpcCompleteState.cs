using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCompleteState : NpcBaseState
{
    float timeTaken = 13f;
    public override void EnterState(NpcStateManager npc)
    {
        Debug.Log("Entered completion state.");
        npc.GetComponentInChildren<Renderer>().material.color = Color.green;
    }

    public override void UpdateState(NpcStateManager npc)
    {
        
    }

    public override void OnCollisionEnter(NpcStateManager npc, Collision collision)
    {

    }

    public int GiveReward()
    {
        if(timeTaken < 20f)
        {
            return 40;
        }
        else if(timeTaken < 40f)
        {
            return 20;
        }
        else
        {
            return 10;
        }
    }
}
