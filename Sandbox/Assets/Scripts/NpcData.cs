using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcData 
{
    public string NpcName;
    public NpcType m_Type;
    public List<string> NpcDialogue = new List<string>();
    public string m_TimeToBeat;

    public static Dictionary<int, NpcData> DataDict = new Dictionary<int, NpcData>();
    public static NpcData Get(int ID) 
    {
        return DataDict[ID];
    } 
}
