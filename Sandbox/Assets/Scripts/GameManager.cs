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

    public PlayerController2 PlayerRef;
    public Transform StartPoint;

    public void Awake()
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
        //Initalize();
    }

    private void Initialize() 
    {   
        PlayerRef.transform.position = StartPoint.position;
    }





}
