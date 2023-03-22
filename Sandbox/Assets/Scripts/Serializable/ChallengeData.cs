using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ChallengeData
{  
    [Header("Challenge Attributes")]
    public int m_ID;
    public string m_Name;
    public string m_Description;
    public string m_Objective;
    public float m_TimeLimit = 600; // 10 minute defualt?
    public float m_Score = 600; 

    [Header("Challenge Components")]
    public int WPS_Index;
}

[Serializable]
public class ChallengeRecord
{
    public int m_ID;
    public float m_Score;
}
