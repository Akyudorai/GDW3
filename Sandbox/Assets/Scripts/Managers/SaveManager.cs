using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Runtime.InteropServices;

public class SaveManager : MonoBehaviour
{
    // -- SINGLETON --
    private static SaveManager instance;
    public static SaveManager GetInstance() 
    {
        return instance;
    }

    #region DLL IMPORTS

    [DllImport("SaveManager.dll")]
    private static extern IntPtr createInstance();
    [DllImport("SaveManager.dll")]
    private static extern void deleteInstance(IntPtr instance);

    [DllImport("SaveManager.dll")]
    private static extern void save(StringBuilder myString, int length);
    [DllImport("SaveManager.dll")]
    private static extern void load(StringBuilder myString, int length);

    #endregion    

    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } 

        instance = this;

        LoadFile();        
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        SaveFile();
    }

    private string GenerateSaveStr() 
    {
        string result = "";
        // Load Race Times
        for (int i = 0; i < RaceManager.GetInstance().raceList.Count; i++) 
        {
            float score = RaceManager.GetInstance().raceList[i].m_Score;
            result += "rs-"+i+"-"+score.ToString("F2") + "\n";
        }
        
        return result;
    }
    
    public void SaveFile()
    {
        string output = GenerateSaveStr();
        StringBuilder str = new StringBuilder(output, output.Length);
        save(str, str.Capacity);

        Debug.Log("SAVING");
    }

    public void LoadFile() 
    {
        Debug.Log("======== LOADING =============");
        // Load the file and make sure reading is working fine
        StringBuilder input = new StringBuilder(10000);
        load(input, input.Capacity);

        string[] lines = input.ToString().Split('\n'); 
        foreach (string l in lines) 
        {
            //Debug.Log("Line: " + l);
            // Split into three strings (Type), (ID), (Value) 
            string[] arr = l.Split('-');
            
            if (arr[0] == "rs") {
                // Update race manager with race times                
                RaceManager.GetInstance().raceList[int.Parse(arr[1])].m_Score = float.Parse(arr[2]);
            }

            if (arr[0] == "b") {
                // Update boolean value in player prefs
                PlayerPrefs.SetInt(arr[1], int.Parse(arr[2]));
            }
        }

        Debug.Log("Player Data successfully loaded");        
    }

}
