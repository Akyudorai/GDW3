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

    public override void Interact(Controller pc, RaycastHit hit)
    {
        //if(vendingIsInteractable == true)
        //{
        //    Debug.Log("Vending Machine already used.");
        //    return;
        //}
        //if(GameManager.GetInstance().pcRef.GetMoney() < cost)
        //{
        //    Debug.Log("Not enough money available to buy energy drink.");
        //    return;
        //}
        Debug.Log("Vending Machine");
        pc.v_HorizontalVelocity = Vector3.zero;
        pc.v_VerticalVelocity = Vector3.zero;
        pc.rigid.velocity = Vector3.zero;

        //Opening Vending Machine Dialogue Box
        UI_Manager.GetInstance().LoadVendingMachineDialogue();

        //if(GameManager.GetInstance().pcRef.GetMoney() < 50f) //hide yes button if player doesn't have enough money
        //{
        //    UI_Manager.GetInstance().VendingYesButton.gameObject.SetActive(false);
        //    UI_Manager.GetInstance().VendingOutputDisplay.text = "<s>Buy a drink for $50?</s> Not enough money available.";
        //}

        UI_Manager.GetInstance().VendingYesButton.onClick.RemoveAllListeners();
        UI_Manager.GetInstance().VendingYesButton.onClick.AddListener(delegate
        {
            if(GameManager.GetInstance().pcRef.GetMoney() < 50f) //change text , turn off all buttons except for the close button
            {
                UI_Manager.GetInstance().VendingOutputDisplay.text = "Not enough money available.";
                UI_Manager.GetInstance().VendingYesButton.gameObject.SetActive(false);
                UI_Manager.GetInstance().VendingNoButton.gameObject.SetActive(false);
                UI_Manager.GetInstance().VendingCloseButton.gameObject.SetActive(true);
                return;
            }
            UI_Manager.GetInstance().EndVendingMachineDialogue(); //hide the vending machine dialogue panel.

            this.gameObject.GetComponentInParent<Animator>().SetBool("Interact", true);
            canAnimator.SetBool("CanThrow", true);
            vendingIsInteractable = true;

            //Play Vending machine interact sound
            FMOD.Studio.EventInstance vendingMachineInteractSfx = SoundManager.CreateSoundInstance(SoundFile.VendingMachine);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(vendingMachineInteractSfx, this.gameObject.transform, GameManager.GetInstance().pcRef.rigid);
            vendingMachineInteractSfx.start();
            vendingMachineInteractSfx.release();

            GameManager.GetInstance().pcRef.RemoveMoney(cost); //updating player wallet.

            int groundLayer = LayerMask.NameToLayer("Ground"); //changing vending machine to non-interactble
            this.gameObject.layer = groundLayer;
        });

         
    }
}
