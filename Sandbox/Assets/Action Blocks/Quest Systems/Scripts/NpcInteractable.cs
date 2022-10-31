using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractable : Interactable
{
    public override void Interact(PlayerController2 controller, RaycastHit hit)
    {
        Debug.Log("Interacted");
        if(hit.collider.gameObject.CompareTag("Player") && this.gameObject.GetComponent<Quest>().isComplete == false)
        {
            if(QuestManager.questManager.activeQuest == null)
            {
                QuestManager.questManager.ActivateQuest(this.gameObject.GetComponent<Quest>());
                //this.gameObject.GetComponent<NpcStateManager>().SwitchState(this.gameObject.GetComponent<NpcStateManager>().WaitState);
            }
            else
            {
                if(QuestManager.questManager.activeQuest.allItemsCollected == true)
                {
                    QuestManager.questManager.QuestComplete();
                    //this.gameObject.GetComponent<NpcStateManager>().SwitchState(this.gameObject.GetComponent<NpcStateManager>().CompleteState);
                }
            }
        }
    }
}
