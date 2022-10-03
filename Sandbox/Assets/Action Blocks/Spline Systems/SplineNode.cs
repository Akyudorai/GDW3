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
	public float value = 0.0f;

	public void Attach(PlayerController2 pc, float pos = 0.0f)
	{
		path.pcRef = pc;
        path.nodeIndex = index;
		pc.currentSpline = path;
		value = pos;
	}

	public void Detatch() 
	{        
		path.pcRef.currentSpline = null;
        Vector3 launchForce = (next.position - position).normalized + Vector3.up;
        launchForce *= path.pcRef.JumpForce;
        path.pcRef.rigid.AddForce(launchForce);
		path.pcRef = null;		
	}

	public void Traverse(float speed, bool isForward = true) 
	{
		if (isForward) value += Time.deltaTime * speed;
		else value -= Time.deltaTime * speed;

		value = Mathf.Clamp(value, 0.0f, 1.0f);

		if (value <= 0.0f && !isForward || value >= 1.0f && isForward) {
			if (next.next != null) {
				next.Attach(path.pcRef, 0.0f);				
			} else {
				Detatch();
			}			
		} 
	}

	public Vector3 GetCurrentPosition()
	{
		// Reverse calculate position based on percentage of spline		  	
		if (next != null) {
			Vector3 newPosition = position - ((position - next.position) * value);
			return newPosition;
		}	

		return Vector3.zero;	
	}
}
