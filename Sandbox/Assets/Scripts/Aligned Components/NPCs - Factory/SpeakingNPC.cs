using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SpeakingNPC : MonoBehaviour
{
    public string message;  // Stores message for the player when they get close

    private void OnTriggerEnter(Collider collider)      // Player enters speech range
    {
        if (collider.gameObject.tag == "Player")
        {
            UI_Manager.GetInstance().ToggleInteraction(true);           // Enables interaction text mesh
            UI_Manager.GetInstance().SetInteractionDisplay(message);    // Sets display to the NPC's message string    
        }    
    }

    private void OnTriggerStay(Collider collider)       // Each frame while player remains in speech range
    {
        if (collider.gameObject.tag == "Player")
        {
            UI_Manager.GetInstance().ToggleInteraction(true);           // Ensures interaction text mesh remains active when in range  
        }  
        
    }

    private void OnTriggerExit(Collider collider)       // Player leaves speech range
    {
        if (collider.gameObject.tag == "Player")
        {
            UI_Manager.GetInstance().ToggleInteraction(false);          // Disables interaction text mesh 
        }  
        
    }
}