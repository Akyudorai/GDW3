using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetLoader
{

    private static bool isInitialized = false;

    public static void Initialize() 
    {
        if (isInitialized) {
            Debug.Log("AssetLoader already initialized"); 
            return;
        } 

        LoadQuestItems();
        LoadRaceItems();

        isInitialized = true;
        Debug.Log("Assets Loader: Initialized!");
    }

    public static void Load(string name, int count = 1, string catagory = "Unorganized")
    {
        GameObject objRef = Resources.Load<GameObject>("Prefabs/" + name);

        GameObject tmp;
        for (int i = 0; i < count; i++) 
        {            
            tmp = GameObject.Instantiate(objRef);
            tmp.SetActive(false);
            AssetManager.GetInstance().Add(name, tmp);
        }
    }

    public static void LoadQuestItems()
    {
        Load("Milk Bottle", 3);
        Load("BatteryQuestItemTemp", 3);       
        Load("Shovel", 3);
        Load("HardHat", 3);
        Load("Cupcake", 3);
        Load("PowerCell", 3);
        Load("CowboyBoots", 3);
    }

    public static void LoadRaceItems() 
    {
        Load("Finish Collider", 1);
    }
}
