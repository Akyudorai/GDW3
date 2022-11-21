using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    // TODO: Create an AssetManager and use this script to create several asset pools (NPCs, Cars, Buildings, etc)
    // When a condition is met, a we can manipualte the asset.  
    // AssetManager should generate its pooled objects when starting the game, as opposed to when loading a scene
    public List<GameObject> pooledObjects = new List<GameObject>();

    public GameObject GetPooledObject() 
    {
        for (int i = 0; i < pooledObjects.Count; i++) 
        {
            if (!pooledObjects[i].activeInHierarchy) 
            {
                return pooledObjects[i];
            }
        }

        Debug.Log("ObjectPool: Pool Overflow! Requesting more objects than pool contains!");
        return null;
    }

    public void HideAll() 
    {
        foreach (GameObject obj in pooledObjects) 
        {
            obj.SetActive(false);
        }
    }
}
