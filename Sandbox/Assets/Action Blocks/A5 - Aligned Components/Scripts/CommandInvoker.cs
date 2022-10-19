using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    // Input Reference
    private InputActions inputRef;
    
    // Holds all our ICommand instances;  we use a dictionary so we can easily access them.
    private Dictionary<string, ICommand> cheatCommands = new Dictionary<string, ICommand>();

    // Start is called before the first frame update
    private void Start()
    {
        // Grab reference to the Input Action from Player Controller
        inputRef = GameManager.GetInstance().pcRef.inputAction;

        // Populate cheat list with all our ICommands
        cheatCommands.Add("Sunrise", new Cheats.Sunrise());
        cheatCommands.Add("Noon", new Cheats.Noon());
        cheatCommands.Add("Sunset", new Cheats.Sunset());
        cheatCommands.Add("Midnight", new Cheats.Midnight());
        cheatCommands.Add("Money+", new Cheats.GiveMoney());
        cheatCommands.Add("Money-", new Cheats.TakeMoney());
        
        // Set up command to toggle the enabling of cheats
        inputRef.Player.EnableCheats.performed += cntxt => ToggleCheats();

        // Set the input bindings to the appropriate cheat command
        inputRef.Cheats.Sunrise.performed += cntxt => cheatCommands["Sunrise"].Execute();
        inputRef.Cheats.Noon.performed += cntxt => cheatCommands["Noon"].Execute();
        inputRef.Cheats.Sunset.performed += cntxt => cheatCommands["Sunset"].Execute();
        inputRef.Cheats.Midnight.performed += cntxt => cheatCommands["Midnight"].Execute();
        inputRef.Cheats.MoneyUp.performed += cntxt => cheatCommands["Money+"].Execute();
        inputRef.Cheats.MoneyDown.performed += cntxt => cheatCommands["Money-"].Execute();
    }

    private void ToggleCheats() 
    {
        Debug.Log("Cheats " + ((!Cheats.CheatsEnabled) ? "True" : "False"));
        Cheats.CheatsEnabled = !Cheats.CheatsEnabled;
        
        if (Cheats.CheatsEnabled) {
            inputRef.Cheats.Enable();
        } else {
            inputRef.Cheats.Disable();
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
