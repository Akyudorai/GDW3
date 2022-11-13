using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.InteropServices;

public class SaveManager : MonoBehaviour
{

    #region DLL IMPORTS

    [DllImport("SaveManager.dll")]
    private static extern IntPtr CreateInstance();

    [DllImport("SaveManager.dll")]
    private static extern IntPtr DeleteInstance(IntPtr instance);

    [DllImport("SaveManager.dll")]
    private static extern bool Save(IntPtr instance, string output);

    [DllImport("SaveManager.dll")]
    private static extern string Load(IntPtr instance);

    #endregion

    private static IntPtr instance;

    private void Awake() 
    {
        instance = CreateInstance();       
        Debug.Log(instance); 
    }

    private void Start()
    {
        //SaveFile("Potato Test Save");
        //LoadFile();
    }
    
    public void SaveFile(string output)
    {
        //Save(instance, output);
    }

    public void LoadFile() 
    {
        //Debug.Log(Load(instance));
    }

    private void OnDestroy()
    {
        DeleteInstance(instance);
    }
}
