using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public List<Transform> spawnLocations;  // Creates a list which can be added to from the inspector
    private int npcIncrement = 0;           // The int is just for creating a unique NPC message for the factory call
    private void Start()
    {
        foreach (Transform spawnLocation in spawnLocations)     // Iterates through list of spawn locations
        {
            // Ideally this code would contain some sort of functionality to make the NPCs say interesting flavor text as the player passes by
            Debug.LogError("Spawn");
            NPCFactory.GetInstance().GetNPC("I am NPC #" + ++npcIncrement + " :)", spawnLocation.position);  // Calling the factory function
        }
    }
}
