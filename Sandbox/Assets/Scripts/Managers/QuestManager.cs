using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public List<GameObject> activeQuestItems = new List<GameObject>();
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

            //outdated
            //UI_Manager.GetInstance().UpdateQuestName(_quest.m_Name);
            //UI_Manager.GetInstance().UpdateQuestDescription(_quest.m_Description);
            //UI_Manager.GetInstance().UpdateQuestObjective(_quest.m_Objective);
            
            GameObject objRef;
            for(int i = 0; i < questList[activeQuestID].m_RequiredItems.Count; i++) //enable the quest items in the game world
            {
                objRef = AssetManager.GetInstance().Get(_quest.m_RequiredItems[i].name);
                activeQuestItems.Add(objRef); //adding active quest item to list of active quest items.
                objRef.transform.position = _quest.m_ItemsPositions[i];
                if (objRef.GetComponent<QuestItem>().itemCollected == false) //checks to see if the object hasn't already been collected.
                {
                    objRef.SetActive(true);
                    Debug.Log("Spawned");
                }

                //Instantiate(questList[activeQuestID].m_RequiredItems[i], questList[activeQuestID].m_ItemsPositions[i].position, Quaternion.identity);
            }

            //Outdated
            // Turn on quest panel when we have a quest
            //UI_Manager.GetInstance().ToggleQuestInfoPanel(true);

            //Outdated
            //Send a notification to the player
            //UI_Manager.GetInstance().FadeInNotification();

            UI_Manager.GetInstance().UpdateQuestStatus("In Progress");
            QuestManager.GetInstance().questList[_quest.m_ID].m_Status = "In Progress";
            QuestManager.GetInstance().questList[_quest.m_ID].m_QuestDataDisplay = selectedQuest;

            selectedQuest._questStatus.text = "In Progress";
            Debug.Log("Quest Activated");
        }        
    }

    public void DeactivateQuest(QuestData _quest)
    {
        //Set active quest id to -1
        activeQuestID = -1;

        //Deactivate all quest item gameobjects
        
        foreach (GameObject obj in activeQuestItems)
        {
            obj.SetActive(false);
        }

        while (activeQuestItems.Count > 0)
        {
            activeQuestItems.RemoveAt(activeQuestItems.Count - 1);
        }


        //Updating interal quest variables and ui
        UI_Manager.GetInstance().UpdateQuestStatus("Available");
        QuestManager.GetInstance().questList[_quest.m_ID].m_Status = "Available";
        QuestManager.GetInstance().questList[_quest.m_ID].m_QuestDataDisplay = selectedQuest; //?? not sure about this line

        selectedQuest._questStatus.text = "Available";
        Debug.Log("Quest Deactivated");
    }

    public void ActivateDeactivateQuest(QuestData _quest)
    {
        if(activeQuestID == -1 && questList[_quest.m_ID].m_Completed == false)
        {
            ActivateQuest(_quest);
        }
        else if(activeQuestID == _quest.m_ID)
        {
            DeactivateQuest(_quest);
        }
    }

    public void DisplayQuestInfo(QuestData _quest)
    {
        UI_Manager.GetInstance().UpdateQuestName(_quest.m_Name);
        UI_Manager.GetInstance().UpdateQuestStatus(_quest.m_Status);
        UI_Manager.GetInstance().UpdateQuestDescription(_quest.m_Description);
        UI_Manager.GetInstance().UpdateQuestObjective(_quest.m_Objective);
        UI_Manager.GetInstance().UpdateQuestHint(_quest.m_Hint);

        for (int i = 0; i < 3; i++) //displays the an image of the quest items in the quest info panel //NEED TO CHANGE THIS LATER //currently being changed
        {
            //UI_Manager.GetInstance().questItemIcons[i].SetActive(true);
            UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().sprite = _quest.m_questItemIcon;
            Color tempColor = UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().color;
            if (i < _quest.m_questItemsCollected) //if item has already been collected set opacity to 1
            {
                tempColor.a = 1f;
                UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().color = tempColor;
            }
            else //if item has not been collected, set opacity to 0.3f
            {
                tempColor.a = 0.3f;
                UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().color = tempColor;
            }
        }

        //check to see if the selected quest is already complete, if so hide the activation toggle
        if(_quest.m_Completed == true)
        {
            UI_Manager.GetInstance().ToggleActivationButton(false);
        }
        else
        {
            UI_Manager.GetInstance().ToggleActivationButton(true);
        }
    }

    public void QuestComplete()
    {
        questList[activeQuestID].m_Completed = true;
        questList[activeQuestID].m_Status = "Complete";
        questList[activeQuestID].m_QuestDataDisplay._questStatus.text = "Complete";

        // Signal the EventManager that a quest was completed
        EventManager.OnQuestComplete?.Invoke(activeQuestID);
        
        activeQuestID = -1;
        UI_Manager.GetInstance().UpdateQuestName("Quest Title: -");
        UI_Manager.GetInstance().UpdateQuestDescription("Quest Description: -");
        UI_Manager.GetInstance().UpdateQuestObjective("Quest Objective: -");

        //GameManager.GetInstance().playerRef.AddMoney(50);

        // Turn off Quest Panel when no quest is left
        UI_Manager.GetInstance().ToggleQuestInfoPanel(false);
        UI_Manager.GetInstance().ToggleQuestListPanel(true);

        UI_Manager.GetInstance().questStatus.text = "Complete";

        UI_Manager.GetInstance().SendNotification("Quest Complete", UI_Manager.GetInstance().questSprite);
    }

    public void QuestItemCollected(QuestItem item)
    {
        if(questList[activeQuestID].m_RequiredItems.Count > 0)
        {
            //update quest app ui
            questList[activeQuestID].m_questItemsCollected += 1; //update internal quest data
            for(int i = 0; i < questList[activeQuestID].m_questItemsCollected; i++)
            {
                Color tempColor = UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().color;
                tempColor.a = 1f;
                UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().color = tempColor;
            }

            QuestItem questObject = questList[activeQuestID].m_RequiredItems[questList[activeQuestID].m_RequiredItems.Count-1];
            questList[activeQuestID].m_RequiredItems.RemoveAt(questList[activeQuestID].m_RequiredItems.Count-1);
            Debug.Log("Items remaining " + questList[activeQuestID].m_RequiredItems.Count);

            UI_Manager.GetInstance().SendNotification("Item Collected", QuestManager.GetInstance().questList[QuestManager.GetInstance().activeQuestID].m_questItemIcon);
        }
        if(questList[activeQuestID].m_RequiredItems.Count == 0)
        {
            questList[activeQuestID].m_RequirementsMet = true;
            QuestComplete();
        }        
    }

    public void ClearSelectedQuest()
    {
        selectedQuest = null;
    }

}
