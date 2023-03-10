using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneSwapInteractable : Interactable
{
    public int sceneIndex = 1;

    public override InteractionType GetInteractionType() 
    {
        return InteractionType.SceneSwap;
    }

    public override void Interact(PlayerController controller, RaycastHit hit)
    {        
        SceneManager.LoadScene(sceneIndex);
    }
}
