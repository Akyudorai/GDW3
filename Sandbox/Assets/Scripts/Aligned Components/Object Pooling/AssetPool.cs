using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPool : MonoBehaviour
{
    // TODO: Create an AssetManager and use this script to create several asset pools (NPCs, Cars, Buildings, etc)
    // When a condition is met, a we can manipualte the asset.  
    // AssetManager should generate its pooled objects when starting the game, as opposed to when loading a scene


    public static AssetPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private void Awake() 
    {
        SharedInstance = this;
    }

    private void Start() 
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++) 
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject() 
    {
        for (int i = 0; i < amountToPool; i++) 
        {
            if (!pooledObjects[i].activeInHierarchy) 
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
