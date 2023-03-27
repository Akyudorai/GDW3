using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationState
{
    public float Movement = 0.0f;    
    public bool IsGrounded = false;
    public bool SplineControl = false;
    public bool IsWallRunning = false;
    public bool IsWallRunningRight = false;
    public bool IsRailGrinding = false;
    public bool IsZiplining = false;
    public bool IsLedgeGrabbing = false;

    public void Set(AnimationState state)
    {
        Movement = state.Movement;
        IsGrounded = state.IsGrounded;
        SplineControl = state.SplineControl;
        IsWallRunning = state.IsWallRunning;
        IsWallRunningRight = state.IsWallRunningRight;
        IsRailGrinding = state.IsRailGrinding;
        IsZiplining = state.IsZiplining;
        IsLedgeGrabbing = state.IsLedgeGrabbing;
    }
}


public class AnimationHandler : MonoBehaviour
{
    public bool IsNetworked;
    public Controller pc;
    public Animator animator;
    public AnimationState currentState;

    public void Initialize(Controller pc, Animator anim, bool isNetworked = false)
    {
        this.pc = pc;
        animator = anim;
        IsNetworked = isNetworked;

        currentState = new AnimationState();
    }

    public void Update()
    {        
        animator.SetFloat("Movement", currentState.Movement);
        animator.SetBool("IsGrounded", currentState.IsGrounded);
        animator.SetBool("SplineControl", currentState.SplineControl);
        animator.SetBool("IsWallrunning", currentState.IsWallRunning);
        animator.SetBool("IsWallrunningRight", currentState.IsWallRunningRight);
        animator.SetBool("IsRailGrinding", currentState.IsRailGrinding);
        animator.SetBool("IsZiplining", currentState.IsZiplining);
        animator.SetBool("IsLedgeGrabbing", currentState.IsLedgeGrabbing);
                
        if (IsNetworked)
        {
            if (Client.IsLocalPlayer(pc.identity))
            {
                ClientSend.SendAnimationState(pc.identity.localClientID, currentState);
            }            
        }
    }

    public void SetAnimationState(AnimationState newState)
    {
        currentState.Set(newState);
    }
}
