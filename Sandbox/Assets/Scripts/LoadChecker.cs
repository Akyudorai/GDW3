using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LoadChecker : MonoBehaviour
{
    private void Awake() 
    {
        if (!GameLoader.IsLoaded) {
            int index = SceneManager.GetActiveScene().buildIndex;    
            SpawnPointManager.currentSceneIndex = index;        
            SceneManager.UnloadSceneAsync(index);
            GameLoader.nextSceneIndex = index;
            SceneManager.LoadSceneAsync(0);
        } else {             
            Destroy(this.gameObject);
        }
    }
}
