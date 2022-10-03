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

    public override void Interact(PlayerController2 pc) 
    {
        // Grab closest point on the collider based on player position          
        Vector3 closestPoint = pc.targetInteractableHitPoint;

        // Calculate percentage of spline                 
        Vector3 pA = node.position;
        Vector3 pB = node.next.position;

        Vector3 nodeToNode = (pA - pB);
        Vector3 nodeToPoint = (pA - closestPoint);
        float result = nodeToPoint.magnitude / nodeToNode.magnitude;
        
        node.Attach(pc, result);        
    }
}
