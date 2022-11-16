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
    public List<Transform> SpawnPoints;

    public bool IsPaused = false;

   

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start() 
    {
        Initialize();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Initialize() 
    {   
        // Load the Player Data
        playerRef = new Player(); // Can replace this with a save/load feature

        // Spawn player at start position
        RespawnPlayer(0);
    }

    public void RespawnPlayer(int overrideIndex = -1)
    {
        Rigidbody pRigid = pcRef.rigid.gameObject.GetComponent<Rigidbody>();        
        pRigid.velocity = Vector3.zero;

        if (overrideIndex == -1) {
            // Teleport to last checkpoint
            pcRef.gameObject.transform.position = SpawnPoints[RespawnIndex].position;
            pcRef.gameObject.transform.rotation = SpawnPoints[RespawnIndex].rotation;
            return;
        }

        else 
        {
            if (overrideIndex > -1 && overrideIndex < SpawnPoints.Count) 
            {
                // Teleport to specified checkpoint
                pcRef.gameObject.transform.position = SpawnPoints[overrideIndex].position;
                pcRef.gameObject.transform.rotation = SpawnPoints[overrideIndex].rotation;
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
