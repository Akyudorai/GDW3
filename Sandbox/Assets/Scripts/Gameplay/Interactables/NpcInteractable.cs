using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractable : Interactable
{
    private NPC npcRef;

    public void SetNpcReference(NPC reference) 
    {
        npcRef = reference;
    }

    public override void Interact(PlayerController controller, RaycastHit hit)
    {
        if (npcRef == null) {
            Debug.LogError("NpcInteractable: NPC Reference not set!");
            return;
        }

        switch (npcRef.m_Type) 
        {
            case NpcType.Standard:
                Debug.Log("This is a standard NPC.");

                // Trigger Dialogue UI

                break;
            case NpcType.Quest_Giver:
                Debug.Log("Trigger Quest: [" + npcRef.m_QuestID + "]");
                GameObject newQuestLogItem = Instantiate(UI_Manager.GetInstance().questLogItem, UI_Manager.GetInstance().contentPanel.transform);
                newQuestLogItem.GetComponent<QuestDataDisplay>().UpdateQuestData(npcRef.m_QuestID);
                
                //if (QuestManager.GetInstance().questList[npcRef.m_QuestID].m_Completed == false)
                //{
                //    NpcStateManager stateManager = this.gameObject.GetComponent<NpcStateManager>();

                //    // Accept the Quest if we don't have one already
                //    if (QuestManager.GetInstance().activeQuestID == -1) 
                //    {
                //        if (QuestManager.GetInstance().questList[npcRef.m_QuestID].m_Completed == false) 
                //        {
                //            QuestManager.GetInstance().ActivateQuest(QuestManager.GetInstance().questList[npcRef.m_QuestID]);
                //            stateManager.SwitchState(stateManager.WaitState);
                //        }                        
                //    } 

                //    else 
                //    {
                //        if (QuestManager.GetInstance().activeQuestID == npcRef.m_QuestID) 
                //        {
                //            if (QuestManager.GetInstance().questList[QuestManager.GetInstance().activeQuestID].m_RequirementsMet == true) 
                //            {
                //                QuestManager.GetInstance().QuestComplete();
                //                stateManager.SwitchState(stateManager.CompleteState);
                //            }
                //        }
                        
                //    }
                //}
                
                break;
            case NpcType.Race_Giver:
                Debug.Log("Trigger Race: [" + npcRef.m_RaceID + "]");
                
                if (RaceManager.GetInstance().m_RaceActive == false) 
                {
                    RaceManager.GetInstance().InitializeRace(controller, npcRef.m_RaceID);
                }

                break;
        }
    }
}
