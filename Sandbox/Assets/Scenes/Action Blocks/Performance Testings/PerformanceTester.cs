using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceTester : MonoBehaviour
{
    public InputActions inputAction;


    public bool usingPooling;



    public ObjectPool testObjectPool;

    public GameObject objectToLoad;
    public int count = 1000;

    public GameObject[] allSpawnedObjects;

    private void Awake() 
    {
        // 
        inputAction = new InputActions();
        inputAction.Player.Jump.performed += cntxt => SpawnObjects();
        inputAction.Player.Interact.performed += cntxt => DestroySpawnedObjects();

        allSpawnedObjects = new GameObject[count];

        if (usingPooling) {
            testObjectPool = new ObjectPool();
            GameObject objRef;
            for (int i = count-1 ; i >= 0; i--) 
            {                
                objRef = Instantiate(objectToLoad, Vector3.zero, Quaternion.identity);
                objRef.SetActive(false);
                testObjectPool.pooledObjects.Add(objRef);                
            }

        }
    }

    private void OnEnable() 
    {
        inputAction.Player.Enable();
    }

    private void OnDisable() 
    {
        inputAction.Player.Disable();
    }

    public void SpawnObjects()
    {
        if (usingPooling) {
            
            for (int i = count-1 ; i >= 0; i--) 
            {                
                testObjectPool.GetPooledObject().SetActive(true);
            }

        }
         else {
            for (int i = count-1 ; i >= 0; i--) 
            {
                allSpawnedObjects[i] = Instantiate(objectToLoad, Vector3.zero, Quaternion.identity);
            }  
        }

              
    }

    public void DestroySpawnedObjects() 
    {
        if (usingPooling) {
            
            for (int i = count-1 ; i >= 0; i--) 
            {
                testObjectPool.HideAll();
            }

        }

        else 
        {
            for (int i = count-1 ; i >= 0; i--) 
            {
                Destroy(allSpawnedObjects[i]);
            }
        }
        
    }
}
