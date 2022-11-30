using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameMode {
        Destination
    }

    private static GameManager instance;
    public static GameManager GetInstance() 
    {
        return instance;
    }

    public Player playerRef;
    public PlayerController pcRef;

    public int RespawnIndex = 0;

    public bool IsPaused = false;

   

    // Initialization
    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } 

        instance = this;
                
        // Load the Player Data 
        playerRef = new Player(); // Can replace this with a save/load feature in future 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        DontDestroyOnLoad(this.gameObject);
    }

    public void RespawnPlayer(int overrideIndex = -1)
    {
        Rigidbody pRigid = pcRef.rigid.gameObject.GetComponent<Rigidbody>();        
        pRigid.velocity = Vector3.zero;

        List<SpawnPoint> points = SpawnPointManager.GetInstance().SpawnPoints[SpawnPointManager.currentSceneIndex];

        if (overrideIndex == -1) {
            // Teleport to last checkpoint
            pcRef.gameObject.transform.position = points[RespawnIndex].Position;
            pcRef.gameObject.transform.rotation = Quaternion.Euler(points[RespawnIndex].EulerRotation);
            return;
        }

        else 
        {
            if (overrideIndex > -1 && overrideIndex < points.Count) 
            {
                // Teleport to specified checkpoint
                pcRef.gameObject.transform.position = points[overrideIndex].Position;
                pcRef.gameObject.transform.rotation = Quaternion.Euler(points[overrideIndex].EulerRotation);
            } else {
                Debug.LogError("RespawnPlayer: No such spawn point with index of [" + overrideIndex + "]");
            }
            
        }
    }


    public void Pause(bool state) 
    {
        Cursor.visible = state;
        Cursor.lockState = (state) ? CursorLockMode.Confined : CursorLockMode.Locked;     
        IsPaused = state;   
    }



}
