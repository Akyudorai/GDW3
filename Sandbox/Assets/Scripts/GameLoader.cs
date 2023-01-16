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
    public static int nextSceneIndex = 2; // TBE -- Change to Menu Scene as Default

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
            SceneManager.LoadScene(nextSceneIndex);
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

        // == UI Manager
        GameObject umObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/UI Manager"), Vector3.zero, Quaternion.identity);

        if (umObj == null) 
        {
            Debug.Log("UI Manager failed to load.");
            return;
        }

        // == ASSET MANAGER
        GameObject amObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/AssetManager"), Vector3.zero, Quaternion.identity);

        if (amObj == null) 
        {
            Debug.LogError("AssetManager failed to load.");
            return;
        }

        // == SPAWN POINT MANAGER
        GameObject spmObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/SpawnPointManager"), Vector3.zero, Quaternion.identity);

        if (spmObj == null) 
        {
            Debug.LogError("SpawnPointManager failed to load.");
            return;
        }

        // == GAME MANAGER
        GameObject gmObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/GameManager"), Vector3.zero, Quaternion.identity);

        if (gmObj == null) 
        {
            Debug.LogError("GameManager failed to load.");
            return;
        }


        // == QUEST MANAGER
        GameObject qmObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/QuestManager"), Vector3.zero, Quaternion.identity);

        if (qmObj == null) 
        {
            Debug.LogError("QuestManager failed to load.");
            return;
        }

        // == RACE MANAGER
        GameObject rmObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/RaceManager"), Vector3.zero, Quaternion.identity);

        if (rmObj == null) 
        {
            Debug.LogError("RaceManager failed to load.");
            return;
        }

        // == SAVE AND LOAD MANAGER
        GameObject slm = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Managers/SaveLoadManager"), Vector3.zero, Quaternion.identity);

        if (slm == null) 
        {
            Debug.LogError("SaveLoadManager failed to load.");
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

        SceneManager.LoadScene(nextSceneIndex);
    }
}
