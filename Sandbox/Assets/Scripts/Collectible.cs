using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType 
{
    TRex,
    MBear,
    Can
}

public class Collectible : MonoBehaviour
{    
    public CollectibleType e_Type;    

    public void OnCollect() 
    {   
        UI_Manager.GetInstance().StartCoroutine(UI_Manager.GetInstance().ToggleCollectiblePanel(true, 3));
        EventManager.OnCollectibleFound?.Invoke((int)e_Type);
        GameObject.Destroy(this.gameObject);        
    }

}
