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

/* Scene Indexes
0 - Loading Scene
1 - Main Menu Scene
2 - Warehouse Interior Scene
3 - City Scene
4 - Parkour Sandbox Scene

*/

public class SpawnPointManager : MonoBehaviour
{  
    public static int currentSceneIndex = 0;
    public static int currentSpawnIndex = 0;

    public Dictionary<int, List<SpawnPoint>> SpawnPoints = new Dictionary<int, List<SpawnPoint>>();
    [SerializeField] private List<SpawnPoint> WarehouseSpawnPoints = new List<SpawnPoint>();
    [SerializeField] private List<SpawnPoint> CitySpawnPoints = new List<SpawnPoint>();
    [SerializeField] private List<SpawnPoint> SandboxSpawnPoints = new List<SpawnPoint>();

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

            SpawnPoints.Add(2, WarehouseSpawnPoints);
            SpawnPoints.Add(3, CitySpawnPoints);
            SpawnPoints.Add(4, SandboxSpawnPoints);
            
        }
    }
}
