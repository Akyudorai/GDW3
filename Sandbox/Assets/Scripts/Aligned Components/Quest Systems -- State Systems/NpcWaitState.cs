using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcWaitState : NpcBaseState
{
    
    public override void EnterState(NpcStateManager npc)
    {
        npc.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public override void UpdateState(NpcStateManager npc)
    {
        
    }

    public override void OnCollisionEnter(NpcStateManager npc, Collision collision)
    {
        //if(QuestManager.questManager.activeQuest.allItemsCollected == true)
        //{
        //    QuestManager.questManager.QuestComplete();
        //    npc.SwitchState(npc.CompleteState);
        //}
    }
}
