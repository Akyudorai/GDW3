using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using TMPro;

public class ControlRebinder : MonoBehaviour
{
    [SerializeField] private InputActionReference key = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private TMP_Text bindingDisplayText = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start() 
    {
        int bindingIndex = key.action.GetBindingIndexForControl(key.action.controls[0]);        

        bindingDisplayText.text = InputControlPath.ToHumanReadableString(
            key.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void StartRebinding() 
    {
        bindingDisplayText.text = "<Press Any Key>";     
        GameManager.GetInstance().pcRef.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = key.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete() 
    {
        GameManager.GetInstance().pcRef.PlayerInput.SwitchCurrentActionMap("Player");
        
        int bindingIndex = key.action.GetBindingIndexForControl(key.action.controls[0]);

        bindingDisplayText.text = InputControlPath.ToHumanReadableString(
            key.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

            
        rebindingOperation.Dispose();
    }
}
