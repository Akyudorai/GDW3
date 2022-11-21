using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneTransitionCollider : MonoBehaviour
{
    public int sceneIndex;
    public int spawnIndex;

    private void OnTriggerEnter(Collider other) 
    {
        int index = SceneManager.GetActiveScene().buildIndex;   
        SpawnPointManager.currentSceneIndex = sceneIndex;         
        SpawnPointManager.currentSpawnIndex = spawnIndex;
        SceneManager.UnloadSceneAsync(index);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
