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
    public List<GameObject> npcList = new List<GameObject>();

    private void Start() 
    {   
        // Turn off quest panel when we dont have a quest
        if (activeQuestID == -1) {
            UI_Manager.GetInstance().ToggleQuestInfoPanel(false);
        }
    }
    public void ActivateQuest(QuestData _quest, QuestDataDisplay _qdd)
    {
        if(questList[_quest.m_ID].m_Completed == false) //activeQuestID == -1 && questList[_quest.m_ID].m_Completed == false
        {
            //activeQuestID = _quest.m_ID;
            
            GameObject objRef;
            for(int i = 0; i < _quest.m_RequiredItems.Count; i++) //enable the quest items in the game world
            {
                objRef = AssetManager.GetInstance().Get(_quest.m_RequiredItems[i].name);
                //activeQuestItems.Add(objRef); //adding active quest item to list of active quest items.
                objRef.transform.position = _quest.m_ItemsPositions[i];
                if (objRef.GetComponent<QuestItem>().itemCollected == false) //checks to see if the object hasn't already been collected.
                {
                    objRef.SetActive(true);
                    Debug.Log("Spawned");
                }
            }

            UI_Manager.GetInstance().UpdateQuestStatus("In Progress");
            QuestManager.GetInstance().questList[_quest.m_ID].m_Status = "In Progress";
            QuestManager.GetInstance().questList[_quest.m_ID].m_QuestDataDisplay = _qdd;

            //_qdd._questStatus.text = "In Progress";
            _qdd._statusImg.sprite = _qdd.statusLabels[1];

        }        
    }

    public void DeactivateQuest(QuestData _quest) //obsolete :(
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

        //selectedQuest._questStatus.text = "Available";
        Debug.Log("Quest Deactivated");
    }

    public void ActivateDeactivateQuest(QuestData _quest) //even more obsolete :((
    {
        if(activeQuestID == -1 && questList[_quest.m_ID].m_Completed == false)
        {
            //ActivateQuest(_quest);
        }
        else if(activeQuestID == _quest.m_ID)
        {
            DeactivateQuest(_quest);
        }
    }

    public void DisplayQuestInfo(QuestData _quest)
    {
        UI_Manager.GetInstance().UpdateQuestName(_quest.m_Name);
        //UI_Manager.GetInstance().UpdateQuestStatus(_quest.m_Status);
        if(_quest.m_Completed == true)
        {
            UI_Manager.GetInstance().UpdateQuestStatusImg(UI_Manager.GetInstance().questLabels[2]); //quest completed label
        }
        else
        {
            UI_Manager.GetInstance().UpdateQuestStatusImg(UI_Manager.GetInstance().questLabels[1]); //quest in progress label
        }
        UI_Manager.GetInstance().UpdateQuestDescription(_quest.m_Description);
        //UI_Manager.GetInstance().UpdateQuestObjective(_quest.m_Objective);
        string _objective = _quest.m_questItemsCollected + "/3 " + _quest.m_questItemName + " collected";
        UI_Manager.GetInstance().UpdateQuestObjective(_objective);
        UI_Manager.GetInstance().UpdateNpcName(_quest.m_npcName);
        UI_Manager.GetInstance().UpdateQuestPfp(_quest.m_pfp);
        //UI_Manager.GetInstance().UpdateQuestHint(_quest.m_Hint);

        for (int i = 0; i < 3; i++) //displays the an image of the quest items in the quest info panel //NEED TO CHANGE THIS LATER //currently being changed
        {
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
    }

    public void QuestComplete(QuestData _quest)
    {
        questList[_quest.m_ID].m_Completed = true;
        questList[_quest.m_ID].m_Status = "Complete";
        //questList[_quest.m_ID].m_QuestDataDisplay._questStatus.text = "Complete";
        questList[_quest.m_ID].m_QuestDataDisplay._statusImg.sprite = questList[_quest.m_ID].m_QuestDataDisplay.statusLabels[2];

        // Signal the EventManager that a quest was completed
        EventManager.OnQuestComplete?.Invoke(_quest.m_ID);
        
        UI_Manager.GetInstance().UpdateQuestName("Quest Title: -");
        UI_Manager.GetInstance().UpdateQuestDescription("Quest Description: -");
        UI_Manager.GetInstance().UpdateQuestObjective("Quest Objective: -");

        // Turn off Quest Panel when no quest is left
        UI_Manager.GetInstance().ToggleQuestInfoPanel(false);
        UI_Manager.GetInstance().ToggleQuestListPanel(true);

        UI_Manager.GetInstance().questStatus.text = "Complete";

        UI_Manager.GetInstance().SendNotification("Quest Complete", UI_Manager.GetInstance().questSprite);
    }

    public void QuestItemCollected(QuestItem item, QuestData _quest)
    {
        if(questList[_quest.m_ID].m_RequiredItems.Count > 0)
        {
            //update quest app ui
            questList[_quest.m_ID].m_questItemsCollected += 1; //update internal quest data
            for(int i = 0; i < questList[_quest.m_ID].m_questItemsCollected; i++)
            {
                Color tempColor = UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().color;
                tempColor.a = 1f;
                UI_Manager.GetInstance().questItemIcons[i].GetComponent<Image>().color = tempColor;
            }

            QuestItem questObject = questList[_quest.m_ID].m_RequiredItems[questList[_quest.m_ID].m_RequiredItems.Count-1];
            questList[_quest.m_ID].m_RequiredItems.RemoveAt(questList[_quest.m_ID].m_RequiredItems.Count-1);
            Debug.Log("Items remaining " + questList[_quest.m_ID].m_RequiredItems.Count);

            DisplayQuestInfo(_quest);
            UI_Manager.GetInstance().SendNotification("Item Collected", QuestManager.GetInstance().questList[_quest.m_ID].m_questItemIcon);
        }
        if(questList[_quest.m_ID].m_RequiredItems.Count == 0)
        {
            questList[_quest.m_ID].m_RequirementsMet = true;

            var npcList = Object.FindObjectsOfType<NPC>();
            for(int i = 0; i < npcList.Length; i++)
            {
                if(npcList[i].GetComponent<NPC>().m_QuestID == _quest.m_ID)
                {
                    npcList[i].gameObject.GetComponent<NpcStateManager>().SwitchState(npcList[i].gameObject.GetComponent<NpcStateManager>().CompleteState);
                }
            }
        }        
    }

    public void ClearSelectedQuest()
    {
        selectedQuest = null;
    }

    public void PlayQuestCompleteSFX()
    {
        FMOD.Studio.EventInstance questCompleteSFX = SoundManager.CreateSoundInstance(SoundFile.QuestComplete);
        questCompleteSFX.start();
        questCompleteSFX.release();
    }

}
