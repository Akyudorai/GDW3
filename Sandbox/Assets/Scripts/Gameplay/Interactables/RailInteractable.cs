using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailInteractable : Interactable
{
    private SplineNode node;

    public void Initialize(SplineNode node) 
    {
        this.node = node;
    }

    public override InteractionType GetInteractionType() 
    {
        return InteractionType.Rail;
    }

    public override void Interact(PlayerController pc, RaycastHit hit) 
    {                      
        // RULE #1: Player must be above the rail to initiate a rail grind
        if (pc.transform.position.y + 1.5f < hit.point.y) return;

        // Reference variables make it easier to type and read
        Vector3 pA = node.position;         
        Vector3 pB = node.next.position;

        // Calculate percentage of spline (percentage = current position on spline / full length of spline) (% = position / length) 
        Vector3 nodeToNode = (pA - pB);
        Vector3 nodeToPoint = (pA - hit.point);
        float result = nodeToPoint.magnitude / nodeToNode.magnitude;
        
        // Calculate Direction of spline
        Vector3 playerRelativeDir = (hit.point + pc.rigid.velocity);                // Player's current direction of travel
        Vector3 surfaceDir = (pB - pA).normalized;                                  // The direction the spline is traveling in

        // Generate a comparison point to see which is closer
        Vector3 pos_dir = hit.point - surfaceDir;                                   // Generate a point in the positive direction 
        Vector3 neg_dir = hit.point + surfaceDir;                                   // Generate a point in the negative direction
        
        // Compare which point is closer to the direction of travel relative to the hit point
        float pos_dist = Vector3.Distance(playerRelativeDir, pos_dir);              
        float neg_dist = Vector3.Distance(playerRelativeDir, neg_dir);
        bool isForward = ((pos_dist >= neg_dist) ? true : false);
    
        // Attach the player to the node at the point of interaction (closest point)
        node.Attach(pc.maneuverHandler.splineController, result, isForward);        

        // Apply a spline boost force to the player
        pc.v_HorizontalVelocity *= 1.2f; // 20%  
        pc.rigid.useGravity = false;

        // Spawn a GrindVFX on the player until detatched
        GameObject newVFX = Instantiate(Resources.Load<GameObject>("VFX/GrindVFX"));
        newVFX.transform.SetParent(pc.gameObject.transform);
        newVFX.transform.localPosition = Vector3.zero;
        newVFX.transform.Rotate(0, pc.transform.rotation.y, 0);
        pc.maneuverHandler.splineController.SplineVFX = newVFX;

        // Play Rail Attach SFX
        FMOD.Studio.EventInstance railAttachSFX = SoundManager.CreateSoundInstance(SoundFile.RailStart);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(railAttachSFX, pc.transform, pc.rigid);
        railAttachSFX.start();
        railAttachSFX.release();    
        
    }
}
