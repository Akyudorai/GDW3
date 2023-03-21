using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class VendingMachineInteractable : Interactable
{
    public Animator canAnimator;
    public bool vendingIsInteractable = false;
    public int cost = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override InteractionType GetInteractionType()
    {
        return InteractionType.VendingMachine;
    }

    public override void Interact(PlayerController controller, RaycastHit hit)
    {
        if(vendingIsInteractable == true)
        {
            Debug.Log("Vending Machine already used.");
            return;
        }
        if(GameManager.GetInstance().pcRef.GetMoney() < cost)
        {
            Debug.Log("Not enough money available to buy energy drink.");
            return;
        }
        Debug.Log("Vending Machine");
        controller.v_HorizontalVelocity = Vector3.zero;
        controller.v_VerticalVelocity = Vector3.zero;
        controller.rigid.velocity = Vector3.zero;

        this.gameObject.GetComponentInParent<Animator>().SetBool("Interact", true);
        canAnimator.SetBool("CanThrow", true);
        vendingIsInteractable = true;

        GameManager.GetInstance().pcRef.RemoveMoney(cost); //updating player wallet.

        int groundLayer = LayerMask.NameToLayer("Ground"); //changing vending machine to non-interactble
        this.gameObject.layer = groundLayer; 
    }


}
