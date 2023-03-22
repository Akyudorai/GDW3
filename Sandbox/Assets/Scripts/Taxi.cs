using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Taxi : MonoBehaviour
{
    public TextMeshProUGUI destination;
    int stopIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        destination.text = "communal steps";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadStops()
    {
        List<Transform> points = SpawnPointManager.GetInstance().SpawnPoints;
        for(int i = 0; i < points.Count; i++)
        {
            Debug.Log(points[i].gameObject.name);
        }
    }

    public void NextStop()
    {
        List<Transform> points = SpawnPointManager.GetInstance().SpawnPoints;
        stopIndex++;
        if(stopIndex >= points.Count)
        {
            stopIndex = 0;
        }
        destination.text = points[stopIndex].gameObject.name;
    }

    public void PreviousStop()
    {
        List<Transform> points = SpawnPointManager.GetInstance().SpawnPoints;
        stopIndex--;
        if (stopIndex < 0)
        {
            stopIndex = points.Count - 1;
        }
        destination.text = points[stopIndex].gameObject.name;
    }

    public void CallTaxi()
    {
        GameManager.GetInstance().RespawnPlayer(stopIndex);
    }
}
