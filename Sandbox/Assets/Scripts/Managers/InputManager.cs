using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputActions inputAction;

    // Singleton Instance
    private static InputManager instance;
    public static InputManager GetInstance() 
    {
        return instance;
    }

    // Initialization
    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } 
     
        instance = this;
        inputAction = new InputActions();        
        DontDestroyOnLoad(this.gameObject);
    }

    public static InputActions GetInput() 
    {
        return inputAction;
    } 

    private void OnEnable() 
    {
        inputAction.Player.Enable();
    }

    private void OnDisable() 
    {
        inputAction.Player.Disable();
    }
}
