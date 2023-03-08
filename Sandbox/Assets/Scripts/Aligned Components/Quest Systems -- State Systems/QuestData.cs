using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;

public enum QuestType 
{
    Fetch
}
/*
 * This class stores everything you need to know about the quests (name, objective, is it complete, etc).
 */

[Serializable]
public class QuestData
{
    [Header("QuestData Attributes")]
    public int m_ID;
    public int m_questItemsCollected; //number of quest items collected
    public string m_Name;
    public string m_Status;
    public string m_Description;
    public string m_Objective;
    public string m_Hint;
    public bool m_Completed;
    public bool m_Collected;
    public QuestDataDisplay m_QuestDataDisplay;
    public Sprite m_questItemIcon;
    public Sprite m_pfp;
    public String m_questItemName;
    public String m_npcName;

    [Header("QuestData Requirements")]
    public QuestType m_Type;
    public List<QuestItem> m_RequiredItems;
    public List<Vector3> m_ItemsPositions;
    public bool m_RequirementsMet; //have the requirements for completing the quest been met?
}
