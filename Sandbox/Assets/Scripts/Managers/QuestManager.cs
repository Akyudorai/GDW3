using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    // -- SINGLETON --
    private static QuestManager instance;
    public static QuestManager GetInstance() 
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // -- END OF SINGLETON --

    [Header("UI Components")]
    public GameObject questPanel;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questObjective;

    [Header("Quest Components")]
    public int activeQuestID = -1;
    public List<QuestData> questList = new List<QuestData>();

    private void Start() 
    {   
        // Turn off quest panel when we dont have a quest
        if (activeQuestID == -1) {
            questPanel.SetActive(false);
        }
    }
    public void ActivateQuest(QuestData _quest)
    {
        if(activeQuestID == -1 && questList[_quest.m_ID].m_Completed == false)
        {
            activeQuestID = _quest.m_ID;
            questTitle.text = _quest.m_Name;
            questDescription.text = _quest.m_Description;
            questObjective.text = _quest.m_Objective;
            
            GameObject objRef;
            for(int i = 0; i < questList[activeQuestID].m_RequiredItems.Count; i++)
            {
                objRef = AssetManager.GetInstance().Get(_quest.m_RequiredItems[i].name);
                objRef.transform.position = _quest.m_ItemsPositions[i].position;
                objRef.transform.rotation = Quaternion.identity;
                objRef.SetActive(true);

                //Instantiate(questList[activeQuestID].m_RequiredItems[i], questList[activeQuestID].m_ItemsPositions[i].position, Quaternion.identity);
            }

            // Turn on quest panel when we have a quest
            questPanel.SetActive(true);

            //Send a notification to the player
            UI_Manager.GetInstance().FadeInNotification();
        }        
    }

    public void QuestComplete()
    {
        questList[activeQuestID].m_Completed = true;
        activeQuestID = -1;
        questTitle.text = "Quest Title: -";
        questDescription.text = "Quest Description: -";
        questObjective.text = "Quest Objective: -";

        GameManager.GetInstance().playerRef.AddMoney(50);

        // Turn off Quest Panel when no quest is left
        questPanel.SetActive(false);
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
