using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeInteractable : Interactable
{
    private SplineNode node;
    public Transform center;
    private bool inverseLookDir;

    public void Initialize(SplineNode node, bool inverseLookDir) 
    {
        this.node = node;
        center = node.path.transform;
        this.inverseLookDir = inverseLookDir;
    }

    public override InteractionType GetInteractionType() 
    {
        return InteractionType.Ledge;
    }

    // ================ GRAB THE LEDGE ============
    public override void Interact(PlayerController controller, RaycastHit hit)
    {
        // RULE #1:  Player.position.y must not be greater than Ledge.position.y
        if (controller.transform.position.y + 1.25f > hit.point.y) return;
        if (controller.b_Grounded) return;

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
        Vector3 look_1 = Quaternion.Euler(0, ((inverseLookDir) ? -90 : 90), 0) * nodeToNode;
        float dist_1 = Vector3.Distance(center.position, grabPosition + look_1);
        Vector3 look_2 = Quaternion.Euler(0, ((inverseLookDir) ? 90 : -90), 0) * nodeToNode;
        float dist_2 = Vector3.Distance(center.position, grabPosition + look_2);                
        Vector3 lookDirection = ((dist_1 < dist_2) ? look_1 : look_2); 
 
        // Set Player Position to the Grab Position
        controller.maneuverHandler.PerformLedgeGrab(grabPosition, lookDirection.normalized);
    }

    
}
