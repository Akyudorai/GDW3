using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string questItemName;

    private void Start()
    {
        questItemName = this.gameObject.name;
    }
    private void OnTriggerEnter(Collider other)
    {
        QuestManager.questManager.QuestItemCollected(this);
        Destroy(this.gameObject);
    }


}
