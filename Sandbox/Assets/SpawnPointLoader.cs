using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointLoader : MonoBehaviour
{
    public List<Transform> SpawnPoints = new List<Transform>();

    public void Awake() 
    {
        SpawnPointManager.GetInstance().SpawnPoints = SpawnPoints;
    }
}
