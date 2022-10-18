using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
   
    InputActions input;
    
    // Start is called before the first frame update
    void Start()
    {
        UI_Manager.GetInstance().timer = UI_Manager.GetInstance().timeDuration;
        input = GameManager.GetInstance().PlayerRef.inputAction;
       
    }

    public void IncreaseCommand()
    {
        ICommand firstCommand = new ScoreCommand();
        firstCommand.IncreaseNum();
    }
    public void DecreaseCommand()
    {
        ICommand secondCommand = new ScoreCommand();
        secondCommand.DecreaseNum();
    }
  
    void Update()
    {
        if (UI_Manager.GetInstance().enableTimer == true)
        {
            if (UI_Manager.GetInstance().timer >= 0)
            {
                UI_Manager.GetInstance().timer -= Time.deltaTime;
                input.Player.IncreaseNum.performed += cntxt => IncreaseCommand();
                input.Player.DecreaseNum.performed += cntxt => DecreaseCommand();
            }
            else
            {
                input.Player.IncreaseNum.Disable();
                input.Player.DecreaseNum.Disable();
                Debug.Log("Cheat Code Disabled");
            }
            ICommand thirdCommand = new ScoreCommand();
            thirdCommand.Execute();
        }
    }
}
