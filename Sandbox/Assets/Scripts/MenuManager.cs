using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{    
    public int citySceneIndex = 3;
    public int tutorialSceneIndex = 2;

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play() 
    {   
        int index = ((SaveManager.Save.TutorialComplete) ? citySceneIndex : tutorialSceneIndex);
        SceneManager.LoadScene(index);
    }
}
