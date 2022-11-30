using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager waypointManager;
    public List<GameObject> waypointPool;
    public int amountToPool;
    public GameObject objectToPool;

    private void Awake()
    {
        if (waypointManager != null && waypointManager != this)
        {
            Destroy(this);
        }
        else
        {
            waypointManager = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        waypointPool = new List<GameObject>();
        GameObject _waypoint;
        for(int i = 0; i < amountToPool; i++)
        {
            _waypoint = Instantiate(objectToPool);
            _waypoint.SetActive(false);
            waypointPool.Add(_waypoint);
        }
    }

    public void ActivateRace(Race _race)
    {
        for(int i = 0; i < _race.raceWaypoints.Count; i++)
        {
            waypointPool[i].SetActive(true);
            waypointPool[i].GetComponent<Transform>().transform.position = _race.raceWaypoints[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
