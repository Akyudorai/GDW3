using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    private void Start() 
    {
        EventManager.OnQuestComplete += OnTutorialQuestCompleted;
    }

    private void OnTutorialQuestCompleted(int ID) 
    {
        // Only applicable when completing specified quests.
        if (ID != 2 && ID != 3 && ID != 4) return;
         
        if (QuestManager.GetInstance().questList[2].m_Completed && 
            QuestManager.GetInstance().questList[3].m_Completed && 
            QuestManager.GetInstance().questList[4].m_Completed)
        {
            Debug.Log("Tutorial Quests Completed!");
            this.gameObject.SetActive(false);
        }
    }
}
