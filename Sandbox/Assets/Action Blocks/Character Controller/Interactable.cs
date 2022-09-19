using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string DebugInteraction = "Test";

    public void Interact() 
    {
        Debug.Log("Interaction: " + DebugInteraction);
    }
}
