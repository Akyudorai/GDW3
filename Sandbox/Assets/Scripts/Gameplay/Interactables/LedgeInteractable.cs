using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeInteractable : Interactable
{
    private SplineNode node;

    public void Initialize(SplineNode node) 
    {
        this.node = node;
    }

    public override InteractableType GetInteractableType() 
    {
        return InteractableType.Manuever;
    }

    // ================ GRAB THE LEDGE ============
    public override void Interact(PlayerController controller, RaycastHit hit)
    {
        // Get reference between two points for easier use
        Vector3 pA = node.position;         
        Vector3 pB = node.next.position;

        // Calculate percentage of spline (percentage = current position on spline / full length of spline) (% = position / length) 
        Vector3 nodeToNode = (pA - pB);
        Vector3 nodeToPoint = (pA - hit.point);
        float result = nodeToPoint.magnitude / nodeToNode.magnitude;

        // Calculate the Grab Position based on percentage of spline determined
        Vector3 grabPosition = pA - (nodeToNode * result);

        // Determine look direction for the grab
        Vector3 lookDirection = Quaternion.Euler(0, 90, 0) * nodeToNode; 
 
        // Set Player Position to the Grab Position
        controller.GrabLedge(grabPosition, lookDirection.normalized);
    }

    
}
