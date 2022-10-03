using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public KeyCode InteractionKey = KeyCode.Space;

    public KeyCode GetInteractionKey() 
    {
        return InteractionKey;
    }

    public abstract void Interact(PlayerController2 controller);
}
