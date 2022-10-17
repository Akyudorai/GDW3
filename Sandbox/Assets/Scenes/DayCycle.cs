using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.InteropServices;

public class DayCycle : MonoBehaviour
{
    #region DLL Imports

    [DllImport("TimeCycleDLL.dll")]
    private static extern IntPtr createDayCycle();

    [DllImport("TimeCycleDLL.dll")]
    private static extern void freeTimeCycle(IntPtr instance);

    [DllImport("TimeCycleDLL.dll")]
    private static extern void tick(IntPtr instance, float deltaTime);

    [DllImport("TimeCycleDLL.dll")]
    private static extern float getTime(IntPtr instance);

    [DllImport("TimeCycleDLL.dll")]
    private static extern void setTime(IntPtr instance, float time);

    #endregion

    private static IntPtr instance;

    public float TimeScale = 1.0f;
    public float CurrentTime = 0.0f;

    private void Start() 
    {
        instance = createDayCycle();               
    }

    private void Update() 
    {
        tick(instance, Time.deltaTime * TimeScale);
        CurrentTime = GetTime();
    }

    public float GetTime() {
        return getTime(instance);
    }

    public void SetTime(float time) 
    {
        setTime(instance, time);
    }

    private void OnDestroy() 
    {        
        freeTimeCycle(instance);
    }

}
