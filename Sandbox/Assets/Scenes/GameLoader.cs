using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private static bool isLoaded = false;
    public static bool IsLoaded { get { return isLoaded; }} 

    public TMP_Text StatusDisplay;
    public TMP_Text VersionDisplay;
    public Image LoadingBar;
    public Image FadeImage; // To be moved to UI Manager when redesigned

    [Header("Debugging")]
    public bool SkipLoadingSequence = false;

    private bool sceneChanging = false;

    private void Start() 
    {
        LoadingBar.fillAmount = 0.0f;
        FadeImage.color = new Color(0, 0, 0, 0);

        GenerateManagers();                
    }

    private void Update() 
    {        
        LoadingBar.fillAmount += Time.deltaTime / 5;
        LoadingBar.fillAmount = Mathf.Clamp(LoadingBar.fillAmount, 0, 1);
    
        if (LoadingBar.fillAmount >= 1.0f && isLoaded && !sceneChanging && !SkipLoadingSequence) {
            StartCoroutine(LoadNextScene());
        } else if (isLoaded && SkipLoadingSequence) {
            SceneManager.LoadScene(1);
        }
    }

    private void GenerateManagers() 
    {
        // == INPUT MANAGER
        GameObject imObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/InputManager"), Vector3.zero, Quaternion.identity);

        if (imObj == null) {
            Debug.LogError("InputManager failed to load.");
            return;
        }

        // == SOUND MANAGER
        GameObject smObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/SoundManager"), Vector3.zero, Quaternion.identity);

        if (smObj == null) {
            Debug.LogError("SoundManager failed to load.");
            return;
        }

        isLoaded = true;        
    }

    private void SetStatus(string output) 
    {
        StatusDisplay.text = output;
    }

    private IEnumerator LoadNextScene() 
    {
        sceneChanging = true;

        while (FadeImage.color.a < 1) 
        {
            FadeImage.color = new Color(0, 0, 0, FadeImage.color.a + Time.deltaTime / 2);
            yield return null;
        } 

        SceneManager.LoadScene(1);
    }
}
