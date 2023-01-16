using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType 
{
    Manuever,
    Social
}

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact(PlayerController controller, RaycastHit hit);
    public abstract InteractableType GetInteractableType();
}
