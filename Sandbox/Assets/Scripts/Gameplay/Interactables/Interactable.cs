using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType 
{   
    Wall,
    Rail,
    Zipline,
    Ledge,
    Social,
    SceneSwap,
    VendingMachine
}

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact(Controller pc, RaycastHit hit);
    //public abstract void Interact(PlayerController controller, RaycastHit hit);
    //public abstract void Interact(NetworkedPlayerController netPC, RaycastHit hit);
    public abstract InteractionType GetInteractionType();    
}
