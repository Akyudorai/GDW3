using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SplineNode
{
	// Identity
	public int index;
	public Vector3 position;
	public SplineNode next; 
	public SplineNode previous;

	// Motion	
    public SplinePath path;	
	public bool IsForward = false;
	public float distanceTraveled = 0.0f;
	public float percentTraveled = 0.0f;
	private float traversalSpeed = 0.0f;

	// Bind our player to the spline and restrict its controls to spline-specific actions
	public void Attach(SplineController sc, float pos = 0.0f, bool isForward = true)
	{
		// Initialize Path Values
		//path.scRef = sc;															// Player Reference
        sc.nodeIndex = index;														// Node Index (of current node)
		IsForward = (path.splineType == SplineType.Zipline) ? true : isForward;		// Direction of Travel
		percentTraveled = pos;														// The point on the spline the player latches onto
		distanceTraveled = GetDistanceToTravel() * pos;		// For measuring distance traveled (important for velocity-based motion along spline)

		// Update Player Values
		sc.currentSpline = path;							// Player's Spline Path Reference
		
		//if (sc.rigid != null) {
		//	sc.rigid.useGravity = false;						// Disable Rigidbody Gravity While Grinding
		//}
	}

	// Increments the traversal point on the spline and checks if we've reached an end
	public void Traverse(SplineController sc, float speed) 
	{
		// Depending on direction of travel, we either increase or decrease the distance traveled.
		distanceTraveled += Time.deltaTime * speed * ((IsForward) ? 1 : -1); 	// The lambda at the end reverses the direction if we're going opposite direction

		// Clamp the distance traveled just so we stick to our path between two nodes and no random teleporting.  Make sure to update percent traveled
		distanceTraveled = Mathf.Clamp(distanceTraveled, 0.0f, GetDistanceToTravel());
		percentTraveled = distanceTraveled / GetDistanceToTravel();

		// Increase or Decrease player velocity if traveling along an angle
		if ((next.position - position).y != 0) {
			//Vector3 direction = (next.position-position).normalized;
			//float force = (next.position-position).y; 
			//path.scRef.ApplyForce(direction * force, ForceMode.Acceleration); 
		}
		
		// Calculate when we reach the end of our node for forward direction
		if (distanceTraveled >= GetDistanceToTravel() && IsForward) 
		{	
			// If we're going forward, check if the next node has a next node.  If it is, we can attach to it (since there's a spline between two connected nodes)
			if (next.next != null) next.Attach(sc, 0.0f, IsForward); 
			else sc.Detatch();		// If there is no next-to-next node, just detatch from the spline (we've reached the end)
		} 

		// Calculate when we reach the end of our node for reverse direction		
		else if (distanceTraveled <= 0.0f && !IsForward) 
		{	
			// If we're going backwards, check if the previous node exists, if there is, we can attach to it (since the current node is the previous' next node)
			if (previous != null) previous.Attach(sc, 1.0f, IsForward);
			else sc.Detatch();		// If there is no previous node, just detatch from the spline (we've reached the end)
		} 
	}

	// Return a position point between the current node's position and the next node's position
	public Vector3 GetCurrentPosition(SplineController sc)
	{
		// Reverse calculate position based on percentage of spline		  	
		if (next != null) {
			Vector3 newPosition = position - (position - next.position) * percentTraveled;
			if (path.splineType == SplineType.Zipline) {
				newPosition -= Vector3.up * (sc.transform.localScale.y*2);
			}
			
			return newPosition;
		}	

		// If not working for whatever reason, just set the player's position to the current node's position
		return position;	
	}

	// Get which way we want to move depending on a boolean
	public Vector3 GetDirection() 
	{
		// return (IsForward) ?
		// 	next.position :
		// 	position;

		//Forward = N[i] - (N[i] - N[i+1])
		//Backward = N[i+1] - (N[i+1] - N[i])
		
		return (IsForward) ?
			position - next.position :
		 	next.position - position;
	}

	// Simply calculates the distance between the current node and the next node. Important for velocity-based motion
	public float GetDistanceToTravel() 
	{
		return (next.position - position).magnitude;
	}
}
