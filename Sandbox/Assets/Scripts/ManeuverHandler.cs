using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 767 -> 678 (LEDGE) -> 607 (SPLINES)
public class ManeuverHandler : MonoBehaviour
{   
    private bool b_IsNetworked = false;

    public Controller pc;
    public Animator animator;    
    public SplineController splineController;

    [Header("Spline Handling")]
    public bool b_IsSplineControlled;
    public bool b_WallRunning;
    public bool b_CanWallRun = false;
    public bool b_RailGrinding;
    public bool b_Ziplining;

    // NOTE TO SELF: 
    /// Current tracks every wall that has been ran on and counts down a timer. Until it can be ran on again 
    /// Considering the idea where instead of tracking every single wall, we just track the most recent wall ran on.
    /// This would allow us to jump between two walls infinitely for extended "hallway" sequences, as the previous wall timer
    /// would get reset to zero once the new wall timer is set.
    /// Alternatively, could keep it as it is now, and add a function where everytime we run on a new wall, we reduce the timer of 
    /// all other timers by X seconds.  Would still encourage chaining jumps between walls, but also still allow us to restrict how 
    /// quickly they can do so. 
    public Dictionary<WallInteractable, float> wallDelays = new Dictionary<WallInteractable, float>();

    [Header("Ledge Handling")]
    public bool b_IsLedgeHandling;
    public bool b_LedgeGrabbing;
    public bool b_LedgeClimbing;
    public bool b_CanLedgeCancel;
    public GameObject anim_RootTracker;



    public void Initialize(Controller pc, Animator animator, bool isNetworked = false) 
    {
        this.pc = pc;
        this.animator = animator;
        b_IsNetworked = isNetworked;

        splineController = GetComponent<SplineController>();
        splineController.Initialize(pc, pc.mesh, true);

        EventManager.OnPlayerLanding += ResetWallInteractionTimers;
        EventManager.OnPlayerLanding += delegate { b_CanWallRun = false; };
    }    

    public void Tick() 
    {   
        b_IsSplineControlled = (splineController.currentSpline != null);                       
        b_WallRunning = b_IsSplineControlled && splineController.currentSpline.splineType == SplineType.Wall;
        b_RailGrinding = b_IsSplineControlled && splineController.currentSpline.splineType == SplineType.Rail;        
        b_Ziplining = b_IsSplineControlled && splineController.currentSpline.splineType == SplineType.Zipline;             
        b_IsLedgeHandling = (b_LedgeGrabbing || b_LedgeClimbing);

        if (b_IsNetworked)
        {
            if (Client.IsLocalPlayer(pc.identity))
            {
                pc.animationHandler.currentState.SplineControl = b_IsSplineControlled;
                pc.animationHandler.currentState.IsRailGrinding = b_RailGrinding;
                pc.animationHandler.currentState.IsZiplining = b_Ziplining;
                pc.animationHandler.currentState.IsWallRunning = b_WallRunning;
                pc.animationHandler.currentState.IsLedgeGrabbing = b_LedgeGrabbing;
            }
        } else
        {
            pc.animationHandler.currentState.SplineControl = b_IsSplineControlled;
            pc.animationHandler.currentState.IsRailGrinding = b_RailGrinding;
            pc.animationHandler.currentState.IsZiplining = b_Ziplining;
            pc.animationHandler.currentState.IsWallRunning = b_WallRunning;
            pc.animationHandler.currentState.IsLedgeGrabbing = b_LedgeGrabbing;
        }
        
        if (b_IsLedgeHandling) 
        {               
            if (b_LedgeGrabbing)
            {
                animator.ResetTrigger("LedgeClimb");
                animator.ResetTrigger("LedgeDrop");
                animator.SetTrigger("LedgeGrab");

                pc.rigid.velocity = Vector3.zero;
            }
        }

        else if (b_IsSplineControlled) 
        {
            if (b_WallRunning) {                
                pc.animationHandler.currentState.IsWallRunningRight = splineController.currentSpline.isRight;
            }            
        }        
        
        // Count down wall delay timers
        HandleWallInteractionTimers();
    }

    public IEnumerator DelayWallRunAllowance() 
    {
        yield return 0;
        b_CanWallRun = true;
    }

    private void HandleWallInteractionTimers() 
    {
        if (wallDelays.Count > 0) 
        {
            var keyList = new List<WallInteractable>();
            
            // We store each interactable in a separate list because we cannot
            // modify the dictionary directly without causing index errors
            foreach (var key in wallDelays.Keys) 
            {
                keyList.Add(key);
            }

            // For each wall, reduce the timer by deltaTime
            foreach (var key in keyList) 
            {
                wallDelays[key] -= Time.deltaTime;

                // Remove any keys with a timer that has finished
                if (wallDelays[key] <= 0) 
                {
                    wallDelays.Remove(key);
                }
            }
        }
    }

    public void ResetWallInteractionTimers() 
    {
        //Debug.Log("Wall Timers Reset");

        if (wallDelays.Count > 0) 
        {
            var keyList = new List<WallInteractable>();
            
            // We store each interactable in a separate list because we cannot
            // modify the dictionary directly without causing index errors
            foreach (var key in wallDelays.Keys)
            {
                keyList.Add(key);
            }

            // Remove from the dictionary every wall interactable
            foreach (var key in keyList) 
            {
                wallDelays.Remove(key);
            }
        }
    }

    public void PerformLedgeGrab(Vector3 position, Vector3 direction) 
    {
        transform.position = position - (Vector3.up * transform.localScale.y * 1.25f);
        b_LedgeGrabbing = true;

        pc.mesh.transform.LookAt(pc.mesh.transform.position + direction);
        pc.v_HorizontalVelocity = Vector3.zero;
        pc.v_VerticalVelocity = Vector3.zero;
        pc.rigid.velocity = Vector3.zero;
        pc.rigid.useGravity = false;

        StartCoroutine(DelayLedgeCancel()); 
    }

    public void PerformLedgeDrop()
    {   
        // Release the Ledge Grab and Fall
        if (b_LedgeGrabbing && b_CanLedgeCancel) 
        {   
            b_LedgeGrabbing = false;
            pc.rigid.useGravity = false;

            animator.SetTrigger("LedgeDrop");
            animator.ResetTrigger("LedgeGrab");
        }       
    }

    public void PerformLedgeClimb() 
    {
        // Trigger the Ledge Climb Animation
        if (b_LedgeGrabbing && b_CanLedgeCancel) 
        {   
            animator.SetTrigger("LedgeClimb");  
            
            b_LedgeGrabbing = false;
            b_CanLedgeCancel = false;
            pc.rigid.useGravity = true;
            pc.ApplyForce(Vector3.up * (pc.f_JumpForce*2));
            
            animator.ResetTrigger("LedgeGrab");
            //b_LedgeGrabbing = false;                
            //b_CanLedgeCancel = false;           
            //StartCoroutine(LedgeClimb());
        }               
    }

    private IEnumerator DelayLedgeCancel() 
    {
        b_CanLedgeCancel = false;
        yield return new WaitForSeconds(0.1f);
        b_CanLedgeCancel = true;

        // Automatic Ledge Climb (removal of ledge cancel)
        PerformLedgeClimb();
    }

    private IEnumerator LedgeClimb() 
    {   
        // Give animation full control over motion
        pc.col.enabled = false;
        pc.rigid.useGravity = false;
        animator.applyRootMotion = true;
        animator.SetTrigger("LedgeClimb");        
        b_LedgeClimbing = true;
        
        yield return new WaitForSeconds(1.45f); // approximate length of ledge climb animation
        //Vector3 rootPos = anim_RootTracker.transform.position;
        //yield return new WaitForSeconds(0.02f);

        // Return control over motion to the player
        animator.SetTrigger("DoneClimbing");
        pc.col.enabled = true;
        animator.applyRootMotion = false;
        animator.ResetTrigger("LedgeGrab");

        // Restore player control
        b_LedgeClimbing = false;
        pc.rigid.useGravity = true;

        animator.ResetTrigger("DoneClimbing");
    }
}
