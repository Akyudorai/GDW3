using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string DebugInteraction = "Test";

    public void Interact(PlayerController2 controller) 
    {
        Debug.Log("Interaction: " + DebugInteraction);        
    }
}
