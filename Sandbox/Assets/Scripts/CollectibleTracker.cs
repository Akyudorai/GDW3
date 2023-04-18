using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleTracker : MonoBehaviour
{
    [Header("Collectible Arrays")]
    public bool[] Mbears = new bool[3];
    public bool[] Rexs = new bool[3];
    public bool[] Cans = new bool[7];
    public bool[] Pumpkins = new bool[3];
    public bool[] Doggos = new bool[3];
    public bool[] Cameras = new bool[3];

    public void Start() 
    {
        EventManager.OnCollectibleFound += CollectibleFound;
        EventManager.OnCollectibleFound += UI_Manager.GetInstance().UpdateCollectibleImage;
        EventManager.OnCollectibleFound += UI_Manager.GetInstance().UpdateCollectibleAnnouncement;
        EventManager.OnCollectibleFound += UI_Manager.GetInstance().UpdateCollectibleUI;
    }

    public void OnDestroy()
    {
        EventManager.OnCollectibleFound -= CollectibleFound;
        EventManager.OnCollectibleFound -= UI_Manager.GetInstance().UpdateCollectibleImage;
        EventManager.OnCollectibleFound -= UI_Manager.GetInstance().UpdateCollectibleAnnouncement;
        EventManager.OnCollectibleFound -= UI_Manager.GetInstance().UpdateCollectibleUI;
    }

    
    public int TotalFound(int ID) 
    {
        int count = 0;

        switch (ID) 
        {   default:
                Debug.Log("NO SUCH COLLECTIBLE WITH THAT ID");
                count = -1;
                break;
            case 0: // REX
                for (int i = 0; i < Rexs.Length; i++) {
                    if (Rexs[i] == true) count++;
                }    
                break;
            case 1: // MBEAR
                for (int i = 0; i < Mbears.Length; i++) {
                    if (Mbears[i] == true) count++;                     
                }
                break;
            case 2: // Can
                for (int i = 0; i < Cans.Length; i++)
                {
                    if (Cans[i] == true) count++;
                }
                break;
            case 3:
                for (int i = 0; i < Pumpkins.Length; i++)
                {
                    if (Pumpkins[i] == true) count++;
                }
                break;
            case 4:
                for (int i = 0; i < Doggos.Length; i++)
                {
                    if (Doggos[i] == true) count++;
                }
                break;
            case 5:
                for (int i = 0; i < Cameras.Length; i++)
                {
                    if (Cameras[i] == true) count++;
                }
                break;
        }

        return count;
    }

    public void CollectibleFound(int ID) 
    {
        switch (ID) 
        {   default:
                Debug.Log("NO SUCH COLLECTIBLE WITH THAT ID");
                break;
            case 0: // REX
                for (int i = 0; i < Rexs.Length; i++) {
                    if (Rexs[i] == false) {
                        Rexs[i] = true;
                        break;
                    }
                }    
                break;
            case 1: // MBEAR
                for (int i = 0; i < Mbears.Length; i++) {
                    if (Mbears[i] == false) {
                        Mbears[i] = true;
                        break;
                    }
                }   
                break;
            case 2: // Can
                for (int i = 0; i < Cans.Length; i++)
                {
                    if (Cans[i] == false)
                    {
                        Cans[i] = true;
                        break;
                    }
                }
                break;
            case 3: // Pumpkin
                for (int i = 0; i < Pumpkins.Length; i++)
                {
                    if (Pumpkins[i] == false)
                    {
                        Pumpkins[i] = true;
                        break;
                    }
                }
                break;
            case 4: // Doggo
                for (int i = 0; i < Doggos.Length; i++)
                {
                    if (Doggos[i] == false)
                    {
                        Doggos[i] = true;
                        break;
                    }
                }
                break;
            case 5: // Camera
                for (int i = 0; i < Cameras.Length; i++)
                {
                    if (Cameras[i] == false)
                    {
                        Cameras[i] = true;
                        break;
                    }
                }
                break;
        }
    }
}
