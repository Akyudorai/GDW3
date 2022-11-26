using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class NPCFactory : MonoBehaviour
{
    public GameObject prefab;   // NPC prefab object goes here

    private static NPCFactory instance = null;

    public static NPCFactory GetInstance()  // Singleton GameObject creation things
    {
        if (instance == null)   // Creating a new factory instance if one doesn't currently exist
        {
            GameObject obj = new GameObject("NPC Factory");
            instance = obj.AddComponent<NPCFactory>();
            //DontDestroyOnLoad(instance);    // Making sure the factory doesn't disappear if we change scenes
        }
        return instance;        // Either way we're giving them back an instance of a factory
    }

    private void Awake() {
        instance = this;                // Set the singleton instance = this
        //DontDestroyOnLoad(instance);    // Redundant check to keep the factory from being destroyed on load
    }

    public SpeakingNPC GetNPC(string npcMessage, Vector3 npcLocation)   // The function that actually makes this a factory class
    {  
        // Using a temporary variable so we can edit the position and message before we return it,
        // seeing as we can't use a constructor here :(
        SpeakingNPC tempNPC = Instantiate(prefab, npcLocation, Quaternion.identity).GetComponent<SpeakingNPC>();
        tempNPC.message = npcMessage;
        return tempNPC; // A brand spankin' new NPC instantiated and returned upon request!
    }
}
