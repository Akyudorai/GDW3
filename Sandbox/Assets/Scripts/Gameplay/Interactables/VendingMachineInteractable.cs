using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class VendingMachineInteractable : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override InteractionType GetInteractionType()
    {
        return InteractionType.VendingMachine;
    }

    public override void Interact(Controller pc, RaycastHit hit)
    {
        Debug.Log("Vending Machine");
        pc.v_HorizontalVelocity = Vector3.zero;
        pc.v_VerticalVelocity = Vector3.zero;
        pc.rigid.velocity = Vector3.zero;

        this.gameObject.GetComponentInParent<Animator>().SetBool("Interact", true);
        Debug.Log("Vending Machine");
    }
}
