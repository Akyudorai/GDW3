using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class NpcInteractable : Interactable
{
    private NPC npcRef;

    public void SetNpcReference(NPC reference) 
    {
        npcRef = reference;
    }

    public override InteractionType GetInteractionType() 
    {
        return InteractionType.Social;
    }

    IEnumerator NpcQuestFinish()
    {        
        this.gameObject.GetComponent<NPC>().npcIcon.GetComponent<Animator>().SetBool("QuestFinish", true);
        yield return new WaitForSeconds(3f);
        this.gameObject.GetComponent<NPC>().npcIcon.SetActive(false);
    }

    public override void Interact(PlayerController controller, RaycastHit hit)
    {
        if (npcRef == null) {
            Debug.LogError("NpcInteractable: NPC Reference not set!");
            return;
        }

        controller.v_HorizontalVelocity = Vector3.zero; 
        controller.v_VerticalVelocity = Vector3.zero; 
        controller.rigid.velocity = Vector3.zero; 

        switch (npcRef.m_Data.m_Type) 
        {
            case NpcType.SceneSwap:
                SceneManager.LoadScene(npcRef.m_SceneIndex);
                return;
                break;
            case NpcType.Standard:                
                // Trigger Dialogue UI
                UI_Manager.GetInstance().LoadNpcDialogue(npcRef);

                // No Button Simply Ends Dialogue
                UI_Manager.GetInstance().NoDialogueButton.onClick.RemoveAllListeners();
                UI_Manager.GetInstance().NoDialogueButton.onClick.AddListener(delegate {
                    UI_Manager.GetInstance().EndNpcDialogue();
                });
                break;
            case NpcType.Quest_Giver:
                Debug.Log("Trigger Quest: [" + npcRef.m_QuestID + "]");
                UI_Manager.GetInstance().LoadNpcDialogue(npcRef);

                // Set the OK button in dialogue to initialize the race
                UI_Manager.GetInstance().YesDialogueButton.onClick.RemoveAllListeners();
                UI_Manager.GetInstance().YesDialogueButton.onClick.AddListener(delegate {

                    // Hide the Dialogue Panel
                    UI_Manager.GetInstance().EndNpcDialogue();

                    if (npcRef.m_QuestID == -1) return;

                    Debug.Log(QuestManager.GetInstance().questList[npcRef.m_QuestID].m_RequirementsMet);

                    if (QuestManager.GetInstance().questList[npcRef.m_QuestID].m_Collected == false) //only adds the quest to the phone, if it isn't already there.
                    {
                        GameObject newQuestLogItem = Instantiate(UI_Manager.GetInstance().questLogItem, UI_Manager.GetInstance().questContentPanel.transform); //add a new quest to the quest app 
                        newQuestLogItem.GetComponent<QuestDataDisplay>().UpdateQuestData(npcRef.m_QuestID); //add the relevant quest info to the new quest
                        QuestManager.GetInstance().questList[npcRef.m_QuestID].m_Collected = true;
                        //UI_Manager.GetInstance().SendNotification("New Quest Received", UI_Manager.GetInstance().questSprite);
                        UI_Manager.GetInstance().SendNotificationV2();
                        QuestManager.GetInstance().ActivateQuest(QuestManager.GetInstance().questList[npcRef.m_QuestID], newQuestLogItem.GetComponent<QuestDataDisplay>());
                        this.gameObject.GetComponent<NpcStateManager>().SwitchState(this.gameObject.GetComponent<NpcStateManager>().WaitState);
                        this.gameObject.GetComponentInChildren<Animator>().SetBool("Interactive", false); //stop mail box sway animation after accepting the quest
                        this.gameObject.GetComponent<NPC>().npcIcon.SetActive(true);
                        this.gameObject.GetComponent<NPC>().npcIcon.GetComponent<Animator>().SetBool("QuestStart", true);
                    }
                });

                UI_Manager.GetInstance().NoDialogueButton.onClick.RemoveAllListeners();
                UI_Manager.GetInstance().NoDialogueButton.onClick.AddListener(delegate {
                    if (QuestManager.GetInstance().questList[npcRef.m_QuestID].m_RequirementsMet == true 
                        && QuestManager.GetInstance().questList[npcRef.m_QuestID].m_Completed == false)
                    {
                        Debug.Log("Quest Complete!!!");
                        QuestManager.GetInstance().QuestComplete(QuestManager.GetInstance().questList[npcRef.m_QuestID]);
                        //this.gameObject.GetComponentInChildren<Animator>().SetBool("Interactive", false); //stop mail sway animation after quest complete
                        StartCoroutine(NpcQuestFinish());
                    }
                    UI_Manager.GetInstance().EndNpcDialogue();
                });



                break;
            case NpcType.Race_Giver:
                Debug.Log("Trigger Race: [" + npcRef.m_RaceID + "]");
                
                if (RaceManager.GetInstance().m_RaceActive == false) 
                {                                                
                    // Open Dialogue UI
                    UI_Manager.GetInstance().LoadNpcDialogue(npcRef);

                    // Set the OK button in dialogue to initialize the race
                    UI_Manager.GetInstance().YesDialogueButton.onClick.RemoveAllListeners();
                    UI_Manager.GetInstance().YesDialogueButton.onClick.AddListener(delegate { 
                        
                        // Hide the Dialogue Panel
                        UI_Manager.GetInstance().EndNpcDialogue();

                        // Begin Cinematic Intro
                        if (npcRef.cinematicIndex != -1) {
                            GameObject.Find("CinematicsManager").GetComponent<CinematicsManager>().Play(npcRef.cinematicIndex);
                        }    

                        // Initialize the Race while cinematic is playing
                        RaceManager.GetInstance().InitializeRace(controller, npcRef.m_RaceID);                         
                    });

                    // No Button Simply Ends Dialogue
                    UI_Manager.GetInstance().NoDialogueButton.onClick.RemoveAllListeners();
                    UI_Manager.GetInstance().NoDialogueButton.onClick.AddListener(delegate {
                        UI_Manager.GetInstance().EndNpcDialogue();
                    });                  
                }

                break;
        }
    }
}
