using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    // Singleton Instance
    private static QuestManager instance;
    public static QuestManager GetInstance() 
    {
        return instance;
    }

    // Initialization
    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } 
     
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // -- END OF SINGLETON --

    [Header("Quest Components")]
    public int activeQuestID = -1;
    public List<QuestData> questList = new List<QuestData>();
    public QuestDataDisplay selectedQuest;

    private void Start() 
    {   
        // Turn off quest panel when we dont have a quest
        if (activeQuestID == -1) {
            UI_Manager.GetInstance().ToggleQuestInfoPanel(false);
        }
    }
    public void ActivateQuest(QuestData _quest)
    {
        if(activeQuestID == -1 && questList[_quest.m_ID].m_Completed == false)
        {
            activeQuestID = _quest.m_ID;
            UI_Manager.GetInstance().UpdateQuestName(_quest.m_Name);
            UI_Manager.GetInstance().UpdateQuestDescription(_quest.m_Description);
            UI_Manager.GetInstance().UpdateQuestObjective(_quest.m_Objective);
            
            GameObject objRef;
            for(int i = 0; i < questList[activeQuestID].m_RequiredItems.Count; i++)
            {
                objRef = AssetManager.GetInstance().Get(_quest.m_RequiredItems[i].name);
                objRef.transform.position = _quest.m_ItemsPositions[i];                
                objRef.SetActive(true);

                //Instantiate(questList[activeQuestID].m_RequiredItems[i], questList[activeQuestID].m_ItemsPositions[i].position, Quaternion.identity);
            }

            // Turn on quest panel when we have a quest
            UI_Manager.GetInstance().ToggleQuestInfoPanel(true);

            //Send a notification to the player
            UI_Manager.GetInstance().FadeInNotification();
        }        
    }

    public void DisplayQuestInfo(QuestData _quest)
    {
        UI_Manager.GetInstance().UpdateQuestName(_quest.m_Name);
        UI_Manager.GetInstance().UpdateQuestDescription(_quest.m_Description);
        UI_Manager.GetInstance().UpdateQuestObjective(_quest.m_Objective);

        for(int i = 0; i < _quest.m_RequiredItems.Count; i++)
        {
            UI_Manager.GetInstance().questItemIcons[i].SetActive(true);
        }
    }

    public void QuestComplete()
    {
        questList[activeQuestID].m_Completed = true;

        // Signal the EventManager that a quest was completed
        EventManager.OnQuestComplete?.Invoke(activeQuestID);
        
        activeQuestID = -1;
        UI_Manager.GetInstance().UpdateQuestName("Quest Title: -");
        UI_Manager.GetInstance().UpdateQuestDescription("Quest Description: -");
        UI_Manager.GetInstance().UpdateQuestObjective("Quest Objective: -");

        GameManager.GetInstance().playerRef.AddMoney(50);

        // Turn off Quest Panel when no quest is left
        UI_Manager.GetInstance().ToggleQuestInfoPanel(false);
    }

    public void QuestItemCollected(QuestItem item)
    {
        if(questList[activeQuestID].m_RequiredItems.Count > 0)
        {
            QuestItem questObject = questList[activeQuestID].m_RequiredItems[questList[activeQuestID].m_RequiredItems.Count-1];
            questList[activeQuestID].m_RequiredItems.RemoveAt(questList[activeQuestID].m_RequiredItems.Count-1);            
        }
        if(questList[activeQuestID].m_RequiredItems.Count == 0)
        {
            questList[activeQuestID].m_RequirementsMet = true;
        }        
    }

}
