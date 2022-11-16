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
            SceneManager.UnloadSceneAsync(index);
            SceneManager.LoadSceneAsync(0);
        } else {
            Destroy(this.gameObject);
        }
    }
}
