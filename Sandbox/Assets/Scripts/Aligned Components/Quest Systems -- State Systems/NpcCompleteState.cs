using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCompleteState : NpcBaseState
{
    public override void EnterState(NpcStateManager npc)
    {
        npc.gameObject.GetComponent<NPC>().m_Data.NpcDialogue[0] = npc.gameObject.GetComponent<NPC>().m_Data.NpcDialogue[2];
    }

    public override void UpdateState(NpcStateManager npc)
    {
        
    }

    public override void OnCollisionEnter(NpcStateManager npc, Collision collision)
    {

    }
}
