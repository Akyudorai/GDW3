using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.InteropServices;

public class TimeCycle : MonoBehaviour
{
    #region DLL Imports

    [DllImport("TimeCycleDll.dll")]
    private static extern IntPtr createDayCycle();

    [DllImport("TimeCycleDll.dll")]
    private static extern IntPtr createTimeCycle(float length);

    [DllImport("TimeCycleDll.dll")]
    private static extern void freeTimeCycle(IntPtr instance);

    [DllImport("TimeCycleDll.dll")]
    private static extern void tick(IntPtr instance, float deltaTime);

    [DllImport("TimeCycleDll.dll")]
    private static extern float getTime(IntPtr instance);

    [DllImport("TimeCycleDll.dll")]
    private static extern void setTime(IntPtr instance, float time);

    [DllImport("TimeCycleDll.dll")]
    private static extern float getCycleLength(IntPtr instance);

    [DllImport("TimeCycleDll.dll")]
    private static extern void setCycleLength(IntPtr instance, float length);

    #endregion

    private static IntPtr instance;

    public float TimeScale = 1.0f;
    public float CurrentTime = 0.0f;

    public Light sun;
    public float degreePerSec;

    public void InitializeDayCycle() 
    {
        instance = createDayCycle();
        degreePerSec = 360 / getCycleLength(instance); 
    }

    public void Tick() 
    {
        tick(instance, Time.deltaTime * TimeScale);
        CurrentTime = GetTime();
        
        sun.transform.Rotate(new Vector3(degreePerSec * TimeScale, 0, 0) * Time.deltaTime);
    }

    public float GetTime() {
        return getTime(instance);
    }

    public void SetTime(float time) 
    {
        setTime(instance, time);
    }

    public float GetCycleLength() {
        return getCycleLength(instance);
    }

    public void SetCycleLength(float length) 
    {
        setCycleLength(instance, length);
    }

    public void Delete() 
    {        
        freeTimeCycle(instance);
    }
}
