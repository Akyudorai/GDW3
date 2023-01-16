using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractable : Interactable
{      
    public override InteractableType GetInteractableType() 
    {
        return InteractableType.Manuever;
    }

    public override void Interact(PlayerController pc, RaycastHit hit) 
    {

        if (pc.b_Grounded) { Debug.Log("IsGrounded"); return; }
        if (pc.v_HorizontalVelocity.magnitude < pc.QuickMaxSpeed/3) { Debug.Log("Too Slow"); return; }    
        if (pc.wallDelays.ContainsKey(this)) { Debug.Log("That Wall Is On Cooldown!"); return; }

        Debug.Log("Wall Run Started");

        // Calculate Direction of spline
        Vector3 playerRelativeDir = (hit.point + pc.v_HorizontalVelocity);                      // Player's current direction of travel
        Vector3 surfaceNormal = hit.normal;                                         // The normal for the surface of the wall
        Vector3 surfaceDir = Quaternion.Euler(0, 90, 0) * surfaceNormal;            // The direction the spline will be traveling        
        
        // Generate a comparison point to see forward or back
        Vector3 pos_dir = hit.point - surfaceDir;                                   // Generate a point in the positive direction 
        Vector3 neg_dir = hit.point + surfaceDir;                                   // Generate a point in the negative direction

        // Compare which point is closer to the direction of travel relative to the hit point
        float pos_dist = Vector3.Distance(playerRelativeDir, pos_dir);              
        float neg_dist = Vector3.Distance(playerRelativeDir, neg_dir);
        bool isForward = ((pos_dist >= neg_dist) ? true : false);        

        // Generate a spline path along the wall to follow and attach the player to it
        float splineSpeed = (pc.v_HorizontalVelocity.magnitude / pc.TopMaxSpeed) * 10;
        
        //float splineSpeed = pc.CurrentSpeed;
        SplinePath wallRunSpline = SplineUtils.GenerateWallRunPath(hit.point + surfaceNormal * 1f, surfaceDir, splineSpeed, isForward);

        pc.mesh.transform.LookAt(pc.mesh.transform.position - surfaceDir * ((isForward) ? -1 : 1));
        wallRunSpline.isRight = (Physics.Raycast(pc.mesh.transform.position, pc.mesh.transform.right, 1));
        wallRunSpline.GetNode(0).Attach(pc.splineController, 0.0f, true);  
                
        pc.wallDelays.Add(this, 3f); // Change value to variable for adjustable wall delay time
    }
}
