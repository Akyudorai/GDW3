using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateManager : MonoBehaviour
{
    NpcBaseState currentState;
    public NpcHelpState HelpState = new NpcHelpState();
    public NpcWaitState WaitState = new NpcWaitState();
    public NpcCompleteState CompleteState = new NpcCompleteState();

    // Start is called before the first frame update
    void Start()
    {
        currentState = HelpState;

        currentState.EnterState(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(NpcBaseState npc)
    {
        currentState = npc;
        npc.EnterState(this);
    }
}
