using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string questItemName;
    public bool itemCollected = false;
    public int questIndex = -1;

    private void Start()
    {
        questItemName = this.gameObject.name;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            QuestManager.GetInstance().QuestItemCollected(this, QuestManager.GetInstance().questList[questIndex]);
            this.gameObject.SetActive(false);
        }        
    }


}
