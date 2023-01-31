using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDataDisplay : MonoBehaviour //attached to a quest button in quest panel.
{
    public int questId = -1;
    public TextMeshProUGUI _questName;
    public TextMeshProUGUI _questStatus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateQuestData(int newId)
    {
        questId = newId;
        _questName.text = QuestManager.GetInstance().questList[newId].m_Name;
        _questStatus.text = "Available";
    }

    public void QuestSelected() //player clicks on a quest from the quest list
    {
        UI_Manager.GetInstance().ToggleQuestListPanel(false);
        UI_Manager.GetInstance().ToggleQuestInfoPanel(true);

        QuestManager.GetInstance().selectedQuest = this;
        QuestManager.GetInstance().DisplayQuestInfo(QuestManager.GetInstance().questList[questId]);

        Debug.Log(questId);
    }
}
