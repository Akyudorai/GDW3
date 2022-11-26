using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    // ----- SINGLETON ----
    private static AssetManager instance;
    public static AssetManager GetInstance() 
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null) { Destroy(this.gameObject);}
        else { instance = this; }

        // Initialize Assets
        ObjectDict = new Dictionary<string, ObjectPool>();
        AssetLoader.Initialize();
        DontDestroyOnLoad(this.gameObject);
    }

    // --------------------

    public GameObject GeneratedObjParent;
    public Dictionary<string, ObjectPool> ObjectDict;

    public void Add(string index, GameObject obj)
    {
        obj.transform.SetParent(GeneratedObjParent.transform);

        // Add to an existing pool
        if (ObjectDict.ContainsKey(index)) 
        {            
            ObjectDict[index].pooledObjects.Add(obj);            
        } 
        
        // Create a new pool if it doesn't exist already
        else 
        {
            ObjectPool newPool = new ObjectPool();
            newPool.pooledObjects.Add(obj); 
            ObjectDict.Add(index, newPool);
        }
    }

    public GameObject Get(string index) 
    {
        if (ObjectDict.ContainsKey(index)) 
        {
            return ObjectDict[index].GetPooledObject();
        } 
        
        else 
        {
            Debug.Log("AssetManager: Object Pool for Object [" + index +"] does not exist.");
            return null;
        }
    }
    

}
