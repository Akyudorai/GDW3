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
    public Transform m_StartPoint;
    public Transform m_EndPoint;
}

