using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum NpcType 
{
    Standard,
    Quest_Giver,
    Race_Giver,
    SceneSwap
}

[RequireComponent(typeof(NpcInteractable))]
public class NPC : MonoBehaviour
{
    [Header("NPC Data")]
    public int m_ID;
    public NpcData m_Data;    
    public int cinematicIndex = -1;

    [Header("Dialogue Elements")]
    public Sprite DialogueImage;
    public Sprite DialogueYesImage;    

    [Header("Quest Data")]
    public int m_QuestID = -1;
    public GameObject npcIcon;
    
    [Header("Race Data")]
    public int m_RaceID = -1;

    [Header("Scene Data")]
    public int m_SceneIndex = 3;

    private void Start() 
    {
        GetComponent<NpcInteractable>().SetNpcReference(this);
        NpcData.DataDict.Add(m_ID, m_Data);
    }

    



}
