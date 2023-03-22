using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointLoader : MonoBehaviour
{
    public List<Transform> SpawnPoints = new List<Transform>();

    public void Awake() 
    {
        if (GameLoader.IsLoaded) {
            SpawnPointManager.GetInstance().SpawnPoints = SpawnPoints;
        }        
    }
}
