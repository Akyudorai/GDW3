using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public bool b_RespawnClosest = true;
    public bool b_RespawnDirect = false;
    public Transform directSpawnPoint;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {            
            if (b_RespawnClosest) 
            {
                int index = 0;
                Transform closestTransform = SpawnPointManager.GetInstance().SpawnPoints[0];
                for (int i = 1; i < SpawnPointManager.GetInstance().SpawnPoints.Count; i++) 
                {
                    float currDist = Vector3.Distance(other.transform.position, closestTransform.position);
                    float newDist = Vector3.Distance(other.transform.position, SpawnPointManager.GetInstance().SpawnPoints[i].position);

                    if (newDist < currDist) {
                        closestTransform = SpawnPointManager.GetInstance().SpawnPoints[i];
                        index = i;
                    }
                }

                GameManager.GetInstance().RespawnPlayer(index);
            } 
            else if(b_RespawnDirect)
            {
                for(int i = 0; i < SpawnPointManager.GetInstance().SpawnPoints.Count; i++)
                {
                    if(SpawnPointManager.GetInstance().SpawnPoints[i] == directSpawnPoint)
                    {
                        GameManager.GetInstance().RespawnPlayer(i);
                        return;
                    }
                }
            }
            else
            {
                // Respawn at Communal Steps (Default)
                GameManager.GetInstance().RespawnPlayer(0);
            }
            
        

        }    
    }
}
