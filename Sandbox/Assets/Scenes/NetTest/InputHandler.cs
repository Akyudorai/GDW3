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

    // Axis Values
    public Vector2 MotionInput;
    public Vector2 CameraInput;

    // Delegate Callback Functions for Input Actions
    public delegate void InputDelegate();
    public InputDelegate MoveCallback;
    public InputDelegate JumpCallback;
    public InputDelegate LookCallback;
    public InputDelegate InteractCallback;
    public InputDelegate WallrunInteractionCallback;
    public InputDelegate ZiplineInteractionCallback;
    public InputDelegate RailInteractionCallback;
    public InputDelegate LedgeInteractionCallback;
    //public InputDelegate LedgeClimbCallback;
    //public InputDelegate LedgeReleaseCallback;

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
        InteractCallback?.Invoke();
    }

    public void WallRunCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        WallrunInteractionCallback?.Invoke();
    }

    public void ZiplineCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        ZiplineInteractionCallback?.Invoke();
    }

    public void RailGrindCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        RailInteractionCallback?.Invoke();
    }

    public void LedgeGrabCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        LedgeInteractionCallback?.Invoke();
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
