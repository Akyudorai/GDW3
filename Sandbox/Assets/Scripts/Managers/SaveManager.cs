using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Runtime.InteropServices;

[System.Serializable]
public class SaveData 
{
    // List of record times for each race
    public float[] RaceScores = new float[5];  

    // true = quest complete, false = quest incomplete
    // Consider changing to a serialized quest data class to track inprogress status
    public bool[] Quests = new bool[13];
    
    // true = collectible has been found, false = collectible not found
    // 3 x number of collectible options: 0,1,2 = collectible1, 3,4,5 = collectible2..
    public bool[] Collectibles = new bool[15];  

    // Check if tutorial has already been completed
    public bool TutorialComplete = false;
}

[System.Serializable]
public class SettingsData 
{
    // Audio
    public float Master_Volume = 1.0f;
    public float BGM_Volume = 1.0f;
    public float SFX_Volume = 1.0f;
    public bool Mute_Audio  = false;

    // Video 
    public bool Fullscreen = true;
    // > Resolution

    // Gameplay
    public float Mouse_Sensitivity = 1.0f;
    public bool Invert_Mouse = false;

    // Keybindings
    // > Find a way to get bindings from input system, probably as a dictionary
    

}

public class SaveManager : MonoBehaviour
{
    // -- SINGLETON --
    private static SaveManager instance;
    public static SaveManager GetInstance() 
    {
        return instance;
    }

    public static SaveData Save;
    private string SaveFilePath;

    public static SettingsData Settings;
    private string SettingsFilePath;

    // #region DLL IMPORTS

    // [DllImport("SaveManager.dll")]
    // private static extern IntPtr createInstance();
    // [DllImport("SaveManager.dll")]
    // private static extern void deleteInstance(IntPtr instance);

    // [DllImport("SaveManager.dll")]
    // private static extern void save(StringBuilder myString, int length);
    // [DllImport("SaveManager.dll")]
    // private static extern void load(StringBuilder myString, int length);

    // #endregion    

    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } 

        instance = this;

        //LoadFile();
        SaveFilePath = Application.persistentDataPath + "/sp_SaveData.json";
        Save = LoadSaveDataFromJson(SaveFilePath);
        if (Save == null) Save = new SaveData();

        SettingsFilePath = Application.persistentDataPath + "/Settings.json";
        Settings = LoadSettingsFromJson(SettingsFilePath);
        if (Settings == null) Settings = new SettingsData(); 

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        //SaveFile(); 
        SavePlayerData();
        SaveSettingsData();
    }

    public void SavePlayerData() 
    {
        // Try to save our client's SaveData to it.
        try {
            System.IO.File.WriteAllText(SaveFilePath, JsonUtility.ToJson(Save, true));
            Debug.Log("SaveManager: PlayerData successfully saved.");
        }

        catch {
            Debug.LogError("SaveManager: Failed to save player data to JSON file.");
        }
    }

    public void SaveSettingsData() 
    {
        // Try to save our client's SettingsData
        try {            
            System.IO.File.WriteAllText(SettingsFilePath, JsonUtility.ToJson(Settings, true));
            Debug.Log("SaveManager: Settings successfully saved.");
        }

        catch {
            Debug.LogError("SaveManager: Failed to save settings data to JSON file.");
        }
    }

    // private T LoadFromJson<T>(string path)
    // {
    //     // If the filePath exists, try to load it into our JSON parser into a native class.
    //     if (System.IO.File.Exists(path)) 
    //     {
    //         try {
    //             return JsonUtility.FromJson<T>(path);
    //         } catch {
    //             Debug.LogError("SaveManager: Failed to load JSON file: " + path);
    //             return default(T);
    //         }
    //     }

    //     // Otherwise, return null;
    //     Debug.LogWarning("SaveManager: FilePath not found: " + path);
    //     return default(T);
    // }

    private SaveData LoadSaveDataFromJson(string path) 
    {
        // If the filePath exists, try to load it into our JSON parser into a native class.
        if (System.IO.File.Exists(path)) 
        {
            try {
                string loadedData = System.IO.File.ReadAllText(path);
                SaveData result = JsonUtility.FromJson<SaveData>(loadedData);
                return result;
            } catch {
                Debug.LogError("SaveManager: Failed to load JSON file: " + path);
                return null;
            }
        }

        // Otherwise, return null;
        Debug.LogWarning("SaveManager: FilePath not found: " + path);
        return null;
    }

    private SettingsData LoadSettingsFromJson(string path) 
    {
        // If the filePath exists, try to load it into our JSON parser into a native class.
        if (System.IO.File.Exists(path)) 
        {
            try {
                string loadedData = System.IO.File.ReadAllText(path);
                SettingsData result = JsonUtility.FromJson<SettingsData>(loadedData); 
                return result;
            } catch {
                Debug.LogError("SaveManager: Failed to load JSON file: " + path);
                return null;
            }
        }

        // Otherwise, return null;
        Debug.LogWarning("SaveManager: FilePath not found: " + path);
        return null;
    }

    // private string GenerateSaveStr() 
    // {
    //     string result = "";

    //     // Load Race Times
    //     for (int rc = 0; rc < RaceManager.GetInstance().raceList.Count; rc++) 
    //     {
    //         float rc_score = RaceManager.GetInstance().raceList[rc].m_Score;
    //         result += "rc-"+rc+"-"+rc_score.ToString("F2") + "\n";
    //     }

    //     return result;
    // }
    
    // public void SaveFile()
    // {
    //     string output = GenerateSaveStr();
    //     StringBuilder str = new StringBuilder(output, output.Length);
    //     save(str, str.Capacity);

    //     Debug.Log("SAVING");
    // }

    // public void LoadFile() 
    // {
    //     Debug.Log("======== LOADING =============");
    //     // Load the file and make sure reading is working fine
    //     StringBuilder input = new StringBuilder(10000);
    //     load(input, input.Capacity);

    //     string[] lines = input.ToString().Split('\n'); 
    //     foreach (string l in lines) 
    //     {
    //         //Debug.Log("Line: " + l);
    //         // Split into three strings (Type), (ID), (Value) 
    //         string[] arr = l.Split('-');
            
    //         if (arr[0] == "rc") {
    //             // Update race manager with race times                
    //             RaceManager.GetInstance().raceList[int.Parse(arr[1])].m_Score = float.Parse(arr[2]);
    //         }

    //         if (arr[0] == "b") {
    //             // Update boolean value in player prefs
    //             PlayerPrefs.SetInt(arr[1], int.Parse(arr[2]));
    //         }
    //     }

    //     Debug.Log("Player Data successfully loaded");        
    // }

}
