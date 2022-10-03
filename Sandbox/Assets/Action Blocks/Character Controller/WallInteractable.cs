using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteractable : Interactable
{      
    public override void Interact(PlayerController2 pc) 
    {
        if (pc.IsJumping) return;
      
        Vector3 moveDirection = (pc.camera_pivot.transform.forward).normalized;
        moveDirection.y = 0;

        Vector3 wallNormalDirection = Vector3.zero;
        if (Physics.Raycast(pc.transform.position, transform.position, out RaycastHit hit, (pc.transform.position - transform.position).magnitude))
        {
            if (hit.collider.gameObject == this.gameObject) 
            {
                wallNormalDirection = hit.normal;
            }
        }        

        // ** TO DO **
        // Rotate wall direction based on direction of travel        
        wallNormalDirection = Quaternion.Euler(0, 90, 0) * wallNormalDirection;

        
        SplinePath wallRunSpline = SplineUtils.GenerateWallRunPath(pc.transform.position, wallNormalDirection.normalized, 3.0f);
        wallRunSpline.GetCurrentNode().Attach(pc, 0.0f);  
    }

    public void OnDrawGizmos() 
    {
                
    }
}
