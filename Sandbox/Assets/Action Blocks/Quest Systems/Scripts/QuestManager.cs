using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;
    private void Awake()
    {
        if(questManager != null && questManager != this)
        {
            Destroy(this);
        }
        else
        {
            questManager = this;
        }
    }

    [Header("UI Components")]
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questObjective;

    [Header("Quest Components")]
    [SerializeField]
    public Quest activeQuest;
    private List<Quest> quests;


    public void ActivateQuest(Quest _quest)
    {
        if(activeQuest == null)
        {
            activeQuest = _quest;
            questTitle.text = _quest.GetQuestName();
            questDescription.text = _quest.GetQuestDescription();
            questObjective.text = _quest.GetQuestObjective();
            for(int i = 0; i < activeQuest.questItems.Count; i++)
            {
                Instantiate(activeQuest.questItems[i]);
            }
        }        
    }

    public void QuestComplete()
    {
        activeQuest.SetQuestCondition(true);
        activeQuest = null;
        questTitle.text = "Quest Title: -";
        questDescription.text = "Quest Description: -";
        questObjective.text = "Quest Objective: -";
    }

    public void QuestItemCollected(QuestItem item)
    {
        if(activeQuest.questItems.Count > 0)
        {
            activeQuest.questItems.RemoveAt(activeQuest.questItems.Count-1);
        }
        if(activeQuest.questItems.Count == 0)
        {
            activeQuest.allItemsCollected = true;
        }        
    }

}
