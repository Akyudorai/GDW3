using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractable : Interactable
{      
    public override void Interact(PlayerController2 pc, RaycastHit hit) 
    {
        if (pc.IsGrounded) return;
              
        // Calculate Direction of spline
        
        Vector3 playerRelativeDir = (hit.point + pc.rigid.velocity);                // Player's current direction of travel
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
        SplinePath wallRunSpline = SplineUtils.GenerateWallRunPath(hit.point + surfaceNormal * 0.5f, surfaceDir, pc.rigid.velocity.magnitude * 2, isForward);
        wallRunSpline.GetCurrentNode().Attach(pc, 0.0f, true);  
    }
}
