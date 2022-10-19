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
    public PlayerController2 pcRef;
    public Transform StartPoint;

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
    }

    private void Initialize() 
    {   
        // Load the Player Data
        playerRef = new Player(); // Can replace this with a save/load feature

        // Spawn player at start position
        //PlayerRef.transform.position = StartPoint.position;
    }



}
