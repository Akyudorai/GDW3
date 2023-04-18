using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType 
{
    TRex,
    MBear,
    Can,
    Pumpkin,
    Doggo,
    Kainat
}

public class Collectible : MonoBehaviour
{    
    public CollectibleType e_Type;    

    public void OnCollect(Controller pc) 
    {           
        if (Client.isConnected)
        {
            if (Client.IsLocalPlayer(pc.identity))
            {
                UI_Manager.GetInstance().StartCoroutine(UI_Manager.GetInstance().ToggleCollectiblePanel(true, 3));
                EventManager.OnCollectibleFound?.Invoke((int)e_Type);
                GameObject.Destroy(this.gameObject);
            }
        }

        else
        {
            UI_Manager.GetInstance().StartCoroutine(UI_Manager.GetInstance().ToggleCollectiblePanel(true, 3));
            EventManager.OnCollectibleFound?.Invoke((int)e_Type);
            GameObject.Destroy(this.gameObject);
        }
              
    }

}
