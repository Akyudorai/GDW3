using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    // Holds all our ICommand instances;  we use a dictionary so we can easily access them.
    private Dictionary<string, ICommand> cheatCommands = new Dictionary<string, ICommand>();

    // Start is called before the first frame update
    private void Start()
    {
        InputActions input = InputManager.GetInput();

        // Populate cheat list with all our ICommands
        cheatCommands.Add("Sunrise", new Cheats.Sunrise());
        cheatCommands.Add("Noon", new Cheats.Noon());
        cheatCommands.Add("Sunset", new Cheats.Sunset());
        cheatCommands.Add("Midnight", new Cheats.Midnight());
        cheatCommands.Add("Money+", new Cheats.GiveMoney());
        cheatCommands.Add("Money-", new Cheats.TakeMoney());
        cheatCommands.Add("Teleport_Apartments", new Cheats.Teleport_Apartments());
        cheatCommands.Add("Teleport_Parking", new Cheats.Teleport_Parking());
        cheatCommands.Add("Teleport_Mall", new Cheats.Teleport_Mall());
        cheatCommands.Add("Teleport_Conyard", new Cheats.Teleport_Conyard());
        cheatCommands.Add("Teleport_Underpass", new Cheats.Teleport_Underpass());
        
        // Set up command to toggle the enabling of cheats
        input.Player.EnableCheats.performed += cntxt => ToggleCheats();

        // Set the input bindings to the appropriate cheat command        
            // Day Cheats
        input.Cheats.Sunrise.performed += cntxt => cheatCommands["Sunrise"].Execute();
        input.Cheats.Noon.performed += cntxt => cheatCommands["Noon"].Execute();
        input.Cheats.Sunset.performed += cntxt => cheatCommands["Sunset"].Execute();
        input.Cheats.Midnight.performed += cntxt => cheatCommands["Midnight"].Execute();        
            // Money Cheats
        input.Cheats.MoneyUp.performed += cntxt => cheatCommands["Money+"].Execute();
        input.Cheats.MoneyDown.performed += cntxt => cheatCommands["Money-"].Execute();
            // Teleportation Cheats
        input.Cheats.TeleportApartments.performed += cntxt => cheatCommands["Teleport_Apartments"].Execute();
        input.Cheats.TeleportParking.performed += cntxt => cheatCommands["Teleport_Parking"].Execute();
        input.Cheats.TeleportMall.performed += cntxt => cheatCommands["Teleport_Mall"].Execute();
        input.Cheats.TeleportConyard.performed += cntxt => cheatCommands["Teleport_Conyard"].Execute();
        input.Cheats.TeleportUnderpass.performed += cntxt => cheatCommands["Teleport_Underpass"].Execute();
    }

    private void ToggleCheats() 
    {
        Debug.Log("Cheats " + ((!Cheats.CheatsEnabled) ? "True" : "False"));
        Cheats.CheatsEnabled = !Cheats.CheatsEnabled;
        
        if (Cheats.CheatsEnabled) {
            InputManager.GetInput().Cheats.Enable();
        } else {
            InputManager.GetInput().Cheats.Disable();
        }
        
    }

    // void Update()
    // {
    //     if (UI_Manager.GetInstance().enableTimer == true)
    //     {
    //         if (UI_Manager.GetInstance().timer >= 0)
    //         {
    //             UI_Manager.GetInstance().timer -= Time.deltaTime;
    //             input.Player.IncreaseNum.performed += cntxt => IncreaseCommand();
    //             input.Player.DecreaseNum.performed += cntxt => DecreaseCommand();
    //         }
    //         else
    //         {
    //             input.Player.IncreaseNum.Disable();
    //             input.Player.DecreaseNum.Disable();
    //             Debug.Log("Cheat Code Disabled");
    //         }
    //         ICommand thirdCommand = new ScoreCommand();
    //         thirdCommand.Execute();
    //     }
    // }
}
