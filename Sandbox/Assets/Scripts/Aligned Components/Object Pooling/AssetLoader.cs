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
        Load("Milk Bottle", 2);
        Load("Shady Package", 4);       
    }

    public static void LoadRaceItems() 
    {
        Load("Finish Collider", 1);
    }
}
