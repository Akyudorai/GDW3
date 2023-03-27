using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    // VARIABLES & COMPONENTS
    // ----------------------------------------------------------------------------

    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;   

    // Identity
    public Controller pc;
    private bool isNetworked = false;

    // Axis Values
    public Vector2 MotionInput;
    public Vector2 CameraInput;

    // Delegate Callback Functions for Input Actions
    public delegate void InputDelegate();
    public delegate void InteractionDelegate(Controller pc, InteractionType type);
    public InputDelegate MoveCallback;
    public InputDelegate JumpCallback;
    public InputDelegate LookCallback;
    public InteractionDelegate InteractCallback;
    public InteractionDelegate WallrunInteractionCallback;
    public InteractionDelegate ZiplineInteractionCallback;
    public InteractionDelegate RailInteractionCallback;
    public InteractionDelegate LedgeInteractionCallback;
    //public InputDelegate LedgeClimbCallback;
    //public InputDelegate LedgeReleaseCallback;

    public void Initialize(Controller pc, bool isNetworked = false)
    {
        this.pc = pc;
        this.isNetworked = isNetworked;
    }

    // ----------------------------------------------------------------------------
    // CONTEXT FUNCTIONS
    // ----------------------------------------------------------------------------

    public void MoveCtx(InputAction.CallbackContext ctx)
    {
        var inputValue = ctx.ReadValue<Vector2>();
        MotionInput = inputValue;
    }

    public void JumpCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        if (isNetworked && !Client.IsLocalPlayer(((NetworkedPlayerController)pc).identity)) return;
        JumpCallback?.Invoke();     
    }

    public void LookCtx(InputAction.CallbackContext ctx)
    {
        var inputValue = ctx.ReadValue<Vector2>();
        CameraInput = inputValue;
    }

    public void InteractCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        pc.interactionHandler.Interact(pc, InteractionType.Social);
        pc.interactionHandler.Interact(pc, InteractionType.VendingMachine);
    }

    public void WallRunCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        pc.interactionHandler.Interact(pc, InteractionType.Wall);
    }

    public void ZiplineCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        pc.interactionHandler.Interact(pc, InteractionType.Zipline);
    }

    public void RailGrindCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        pc.interactionHandler.Interact(pc, InteractionType.Rail);
    }

    public void LedgeGrabCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        pc.interactionHandler.Interact(pc, InteractionType.Ledge);
    }

    public void ToggleMenu(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        if (isNetworked)
        {
            if (Client.IsLocalPlayer(pc.identity))
            {
                UI_Manager.GetInstance().TogglePhonePanel(!UI_Manager.GetInstance().PhonePanel.activeInHierarchy);
            }
        } else
        {
            UI_Manager.GetInstance().TogglePhonePanel(!UI_Manager.GetInstance().PhonePanel.activeInHierarchy);
        }
    }

    //public void LedgeClimbCtx(InputAction.CallbackContext ctx)
    //{
    //    if (!ctx.performed) { return; }
    //    LedgeClimbCallback?.Invoke(ctx);
    //}

    //public void LedgeReleaseCtx(InputAction.CallbackContext ctx)
    //{
    //    if (!ctx.performed) { return; }
    //    LedgeReleaseCallback?.Invoke(ctx);
    //}
}
