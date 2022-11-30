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

    [Header("Race Components")]
    public Vector3 m_StartPosition;
    public Vector3 m_StartRotation;
    public Vector3 m_EndPosition;

    public int WPS_Index;
    public List<Vector3> m_Checkpoints;
}

[Serializable] 
public class RaceRecord
{
    public int m_ID;
    public float m_Score;
}
