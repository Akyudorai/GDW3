using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;

public enum QuestType 
{
    Fetch
}

[Serializable]
public class QuestData
{
    [Header("QuestData Attributes")]
    public int m_ID;
    public string m_Name;
    public string m_Status;
    public string m_Description;
    public string m_Objective;
    public string m_Hint;
    public bool m_Completed;
    public bool m_Collected;
    public QuestDataDisplay m_QuestDataDisplay;

    [Header("QuestData Requirements")]
    public QuestType m_Type;
    public List<QuestItem> m_RequiredItems;
    public List<Vector3> m_ItemsPositions;
    public bool m_RequirementsMet;
}
