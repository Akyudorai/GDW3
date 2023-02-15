using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public string questItemName;
    public bool itemCollected = false;

    private void Start()
    {
        questItemName = this.gameObject.name;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            QuestManager.GetInstance().QuestItemCollected(this);
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }        
    }


}
