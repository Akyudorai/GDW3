using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

//920
public class InteractionHandler : MonoBehaviour
{
    //private bool b_IsNetworked = false;
    private Controller pc;
    //private PlayerController pc;
    //private NetworkedPlayerController netPC;

    public LayerMask InteractableLayer;
    public float InteractionDistance;

    public Interactable lastInteractable;
    public float interactionDelay = 0f;

    [Header("Debugging")]
    public bool DEBUG_InteractionRadius;
    public bool DEBUG_InteractionTarget;
    public Vector3 DEBUG_InteractionTargetPoint;

    public void Initialize(Controller pc)
    {
        this.pc = pc;
    }

    public void Tick() 
    {
        TrackInteractionPrompts();


        // Count down the interaction delay
        if (interactionDelay > 0f && pc.maneuverHandler.splineController.currentSpline == null)
        {
            interactionDelay -= Time.deltaTime;
            interactionDelay = Mathf.Clamp(interactionDelay, 0, 100);
        }    
    }


    /// TODO : Remove differentiation between netPC and pc
    public void TrackInteractionPrompts() 
    {
        Vector3 castPoint = Vector3.zero;
        castPoint = pc.transform.position + pc.transform.up * 1.5f;        
        Collider[] hitColliders = Physics.OverlapSphere(castPoint, InteractionDistance, InteractableLayer);

        GameObject targetInteractable = null;        
        Collider closestHit = null;
        foreach (var hit in hitColliders) 
        {
            Interactable i = hit.gameObject.GetComponent<Interactable>();

            // If no interactable has been set yet, set the first one we find
            if (targetInteractable == null) {
                targetInteractable = hit.gameObject;
                closestHit = hit;
            }

            // Otherwise, compare distances to find which is closer
            else 
            {
                float dist_target = Vector3.Distance(castPoint, closestHit.ClosestPoint(castPoint));
                float dist_compare = Vector3.Distance(castPoint, hit.ClosestPoint(castPoint));

                // If the compared distance is closer than current target, replace it
                if (dist_compare < dist_target) {
                    targetInteractable = hit.gameObject;
                    closestHit = hit;
                }
            }
        }

        PlayerState playerState = pc.e_State;
        SplinePath currentSpline = pc.maneuverHandler.splineController.currentSpline;
        

        if (targetInteractable == null || playerState != PlayerState.Active || currentSpline != null) 
        {
            UI_Manager.GetInstance().TogglePrompt(false);
        }

        else 
        {
            if (Vector3.Distance(castPoint, closestHit.ClosestPoint(castPoint)) <= InteractionDistance)
            {                   
                InteractionType type = targetInteractable.GetComponent<Interactable>().GetInteractionType();
                InputAction key = GetInputActionReference(pc, type);
                
                // Apply the same rules that a player undergoes when determining if a maneuver is possible.
                bool canPerform = true;
                switch (type) 
                {
                    case InteractionType.Rail:
                        if (pc.transform.position.y + 1.5f < closestHit.ClosestPoint(castPoint).y) canPerform = false;
                        if (pc.maneuverHandler.splineController.currentSpline != null) canPerform = false;
                        break;
                    case InteractionType.Wall:
                        if (!pc.maneuverHandler.b_CanWallRun) canPerform = false;
                        if (pc.v_HorizontalVelocity.magnitude < pc.f_AccelerationSpeed / 3) canPerform = false;
                        break;
                    case InteractionType.Ledge:
                        if (pc.transform.position.y + 1.25f > closestHit.ClosestPoint(castPoint).y) canPerform = false;
                        if (pc.b_IsGrounded) canPerform = false;
                        if (pc.maneuverHandler.b_LedgeGrabbing || pc.maneuverHandler.b_LedgeClimbing) canPerform = false;
                        break;
                    case InteractionType.Zipline:
                        break;                   
                }

                // Set the key information as a string for prompt display
                if (key != null && canPerform) {
                    string promptText = InputControlPath.ToHumanReadableString(
                        key.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                    UI_Manager.GetInstance().TogglePrompt(true, promptText);
                }

                // Set debug information
                DEBUG_InteractionTargetPoint = closestHit.ClosestPoint(castPoint);
            }
        }

    }

    public void Interact(Controller pc, InteractionType type)
    {
        Vector3 castPoint = pc.transform.position + pc.transform.up * 1.5f;
        Collider[] hitColliders = Physics.OverlapSphere(castPoint, InteractionDistance, InteractableLayer);

        Interactable targetInteractable = null;
        Collider closestHit = null;
        foreach (var hit in hitColliders)
        {
            Interactable i = hit.gameObject.GetComponent<Interactable>();

            if (i.GetInteractionType() == type)
            {
                // If no interactable has been set yet, set the first one we find.
                if (targetInteractable == null)
                {
                    targetInteractable = i;
                    closestHit = hit;
                }

                // Otherwise, compare distances to find out which is closer
                else
                {
                    float dist_target = Vector3.Distance(castPoint, closestHit.ClosestPoint(castPoint));
                    float dist_compare = Vector3.Distance(castPoint, hit.ClosestPoint(castPoint));

                    // If the compared distance is closer than current target, replace it
                    if (dist_compare < dist_target)
                    {
                        targetInteractable = i;
                        closestHit = hit;
                    }
                }
            }
        }

        if (targetInteractable != null && closestHit != null)
        {
            if (lastInteractable == targetInteractable && interactionDelay > 0)
            {
                Debug.Log("Cannot use interactable that frequently!");
                return;
            }

            if (targetInteractable.gameObject == closestHit.gameObject)
            {
                if (Physics.Raycast(castPoint, closestHit.ClosestPoint(castPoint) - castPoint, out RaycastHit hitResult))
                {
                    Debug.Log("Netwroked Interaction)");
                    targetInteractable.Interact(pc, hitResult);
                    lastInteractable = targetInteractable;
                    interactionDelay = 0.5f;
                }
            }
        }
    }

    private InputAction GetInputActionReference(Controller pc, InteractionType type) 
    {
        InputAction action = null;
        switch (type) 
        {
            case InteractionType.Wall:
                action = pc.inputHandler.PlayerInput.actions["WallRun"];
                break;
            case InteractionType.Rail:
                action = pc.inputHandler.PlayerInput.actions["RailGrind"];
                break;
            case InteractionType.Ledge:
                action = pc.inputHandler.PlayerInput.actions["Ledge Grab"];
                break;
            case InteractionType.Zipline:
                action = pc.inputHandler.PlayerInput.actions["Zipline"];
                break;
            case InteractionType.Social:
                action = pc.inputHandler.PlayerInput.actions["Interact"];
                break;
            case InteractionType.VendingMachine:                
                action = pc.inputHandler.PlayerInput.actions["Interact"];
                break;
        } 

        return action;
    }

    private void OnDrawGizmos() 
    {
       

        if (DEBUG_InteractionRadius) {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + transform.up * 1.5f, InteractionDistance);             
        }

        if (DEBUG_InteractionTarget) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + transform.up * 1.5f, DEBUG_InteractionTargetPoint);
        }        
    }
}
