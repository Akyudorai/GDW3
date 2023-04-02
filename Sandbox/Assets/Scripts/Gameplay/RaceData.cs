using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class RaceData
{
    [Header("Race Attributes")]
    public int m_ID;
    public string m_Name;
    public string m_Description;
    public string m_Objective;    
    public float m_Score = 0;
    public List<float> raceTimes;

    [Header("Race Components")]
    public int WPS_Index;
    
}

[Serializable] 
public class RaceRecord
{
    public int m_ID;
    public float m_Score;
}
