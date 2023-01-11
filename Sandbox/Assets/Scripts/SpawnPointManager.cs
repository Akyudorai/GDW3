using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SpawnPoint 
{
    public Vector3 Position;
    public Vector3 EulerRotation;
}

public class SpawnPointManager : MonoBehaviour
{  
    public static int currentSceneIndex = 0;
    public static int currentSpawnIndex = 0;

    public Dictionary<int, List<SpawnPoint>> SpawnPoints = new Dictionary<int, List<SpawnPoint>>();
    [SerializeField] private List<SpawnPoint> WarehouseSpawnPoints = new List<SpawnPoint>();
    [SerializeField] private List<SpawnPoint> CitySpawnPoints = new List<SpawnPoint>();
    [SerializeField] private List<SpawnPoint> SandboxSpawnPoints = new List<SpawnPoint>();
    [SerializeField] private List<SpawnPoint> ParkourSpawnPoints = new List<SpawnPoint>();

    // WAREHOUSE
    /*
        0 = Warehouse (Front Door)
        1 = Warehouse (Garage Door 1)
        2 = Warehouse (Garage Door 2)
    */

    // CITY
    /*  0 = Apartments
        1 = Warehouse (Front Door)
        2 = Warehouse (Garage Door 1)
        3 = Warehouse (Garage Door 2)
    */

    // SANDBOX
    /*
        0 = default
    */

    // -- SINGLETON --
    private static SpawnPointManager instance;
    public static SpawnPointManager GetInstance() 
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

            SpawnPoints.Add(1, WarehouseSpawnPoints);
            SpawnPoints.Add(2, CitySpawnPoints);
            SpawnPoints.Add(3, SandboxSpawnPoints);
            SpawnPoints.Add(4, ParkourSpawnPoints);

        }
    }
}
