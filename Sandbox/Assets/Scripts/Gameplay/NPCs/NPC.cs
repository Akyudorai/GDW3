using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType 
{
    Standard,
    Quest_Giver,
    Race_Giver
}

[RequireComponent(typeof(NpcInteractable))]
public class NPC : MonoBehaviour
{
    [Header("NPC Data")]
    public NpcType m_Type;

    [Header("Quest Data")]
    public int m_QuestID = -1;
    
    [Header("Race Data")]
    public int m_RaceID = -1;

    private void Start() 
    {
        GetComponent<NpcInteractable>().SetNpcReference(this);
    }

    



}
