using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcBaseState
{
    public abstract void EnterState(NpcStateManager npc);

    public abstract void UpdateState(NpcStateManager npc);

    public abstract void OnCollisionEnter(NpcStateManager npc, Collision collision);
}
