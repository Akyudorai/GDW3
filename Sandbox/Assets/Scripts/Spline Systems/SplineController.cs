using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineController : MonoBehaviour
{
    [Header("Splines")]
    public SplinePath currentSpline = null;
    public int nodeIndex = 0;
    public GameObject mesh;
    //public Rigidbody rigid;

    float traversalSpeed = 1f;
    public PlayerController pcRef;
    //public bool isActive = false;     

    public GameObject SplineVFX = null;

    FMOD.Studio.EventInstance railSFX;
    FMOD.Studio.EventInstance ziplineSFX;

    private void Start() 
    {
        railSFX = SoundManager.CreateSoundInstance(SoundFile.RailLoop);
        ziplineSFX = SoundManager.CreateSoundInstance(SoundFile.ZiplineLoop);
        
        railSFX.start();
        ziplineSFX.start();

        railSFX.setPaused(true);
        ziplineSFX.setPaused(true);
    }

    private void OnDestroy() 
    {
        railSFX.release();
        ziplineSFX.release();
    }

    private void Update() 
    {
        if (currentSpline != null) 
        {            
            transform.position = currentSpline.GetNode(nodeIndex).GetCurrentPosition(this);
            Vector3 lookDir = currentSpline.GetNode(nodeIndex).GetDirection();
            lookDir.y = 0;
            mesh.transform.LookAt(mesh.transform.position - lookDir);
            currentSpline.GetNode(nodeIndex).Traverse(this, traversalSpeed);               
            
            // // Check if wall is ahead
            // bool checkWall = (Physics.Raycast(mesh.transform.position, mesh.transform.forward, 1));
            
            // // If a wall was detected, detatch from spline            
            // if (checkWall) 
            // {
            //     Detatch();
            // }

            if (currentSpline == null) return;

            // Play the audio
            if (currentSpline.splineType == SplineType.Rail) 
            {
                railSFX.setPaused(false);   
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(railSFX, pcRef.transform, pcRef.rigid);
            } 

            else if (currentSpline.splineType == SplineType.Zipline) 
            {
                ziplineSFX.setPaused(false);   
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(ziplineSFX, pcRef.transform, pcRef.rigid);
            }

            if (currentSpline.splineType == SplineType.Wall)
            {
                CheckWall();
            } 
        }

        else 
        {
            railSFX.setPaused(true);
            ziplineSFX.setPaused(true); 
        }

        
    }

    public void CheckWall() 
    {
        if (pcRef != null) 
        {
            // Check to see if we're still running on the wall or if we've reached the end of it
            bool checkWall = (Physics.Raycast(mesh.transform.position, mesh.transform.right * ((currentSpline.isRight) ? 1 : -1), 2f));				            
            // If we reached the end of the wall
            if (!checkWall) 
            {
                Debug.Log("Wall No Longer Detected!");                

                // Detatch ourselves
                Detatch();
            }
        } 
    }
    
    public void Detatch() 
    {
        // Remove the Spline Reference so controller can revert back to normal controls
        // But keep a separate reference to the spline path so we can call the OnDetatched function
        SplinePath pathRef = currentSpline;
        currentSpline = null;

        if (SplineVFX != null) {
            Destroy(SplineVFX);
            SplineVFX = null;
        }

        //if (rigid != null) 
		//{			
			// Re-enable gravity so we can jump off
		//	rigid.useGravity = true;	 
		//}	

        // Only want to add a launch force if it's the player
		if (pcRef != null) 
		{
			// Calculate a launch direction based on spline type
			Vector3 launchDirection = Vector3.zero;
			switch (pathRef.splineType) {
				default:
				// Rail launches player upwards
				case SplineType.Rail: 
                    launchDirection = Vector3.up;	
                    
                    // Play Rail Release SFX
                    FMOD.Studio.EventInstance railReleaseSFX = SoundManager.CreateSoundInstance(SoundFile.RailRelease);
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(railReleaseSFX, pcRef.transform, pcRef.rigid);
                    railReleaseSFX.start();
                    railReleaseSFX.release();
                    break;
				// Zipline launches has the player drop donwards
				case SplineType.Zipline: 
                    launchDirection = -Vector3.up; 
                    
                    // Play Zipline Release SFX
                    FMOD.Studio.EventInstance ziplineReleaseSFX = SoundManager.CreateSoundInstance(SoundFile.ZiplineRelease);
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(ziplineReleaseSFX, pcRef.transform, pcRef.rigid);
                    ziplineReleaseSFX.start();
                    ziplineReleaseSFX.release();                    
                    break;
				// Walls launch the player in the direction of their normal
				case SplineType.Wall: 				
					Debug.Log("Is Right (" + pathRef.isRight + ")");				
					Vector3 normalLaunch = Quaternion.Euler(0, ((pathRef.isRight) ? 90 : -90), 0) * pathRef.GetNode(nodeIndex).GetDirection().normalized;
					normalLaunch.y = 0;
					//Debug.Log(normalLaunch);
					Vector3 verticalLaunch = Vector3.up;
					launchDirection = normalLaunch.normalized * 3.0f + verticalLaunch; 

                    // Play Wallrun Release SFX
                    FMOD.Studio.EventInstance wallReleaseSFX = SoundManager.CreateSoundInstance(SoundFile.WallRunRelease);
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(wallReleaseSFX, pcRef.transform, pcRef.rigid);
                    wallReleaseSFX.start();
                    wallReleaseSFX.release();  
					break;
			}
            
            // Calculate a launch force based on direction of travel.  The more velocity we have, the further we jump.
            Vector3 launchForce = (pathRef.GetNode(nodeIndex).IsForward) ? 				
				(pathRef.GetNode(nodeIndex).next.position - pathRef.GetNode(nodeIndex).position).normalized + launchDirection  : 
				(pathRef.GetNode(nodeIndex).position - pathRef.GetNode(nodeIndex).next.position).normalized  + launchDirection;
			
            //Debug.Log(launchForce);

			// Modify player velocity and direction
			pcRef.v_VerticalVelocity = Vector3.zero;					// Zero-out velocity so our launch force takes over
			pcRef.ApplyForce(launchForce * pcRef.JumpForce);	// Apply launch force to the player
            pcRef.StartCoroutine(pcRef.JumpDelay());
			mesh.transform.rotation = Quaternion.identity;	// Rotate the mesh in the direction of travel
			//pcRef.interactionHandler.targetInteractable = null;						
            pcRef.interactionHandler.interactionDelay = 0.5f;
		} 

        // Lastly, call the OnDetatched method from the spline path to allow it to handle itself
        pathRef.Detatch();

    }

    public void SetTraversalSpeed(float newSpeed) 
    {
        traversalSpeed = newSpeed;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (currentSpline != null) 
        {
            Debug.Log("Collided with " + other.gameObject.name + " while on a spline.");  
            Detatch();
        }          
    }

    private void OnDrawGizmos() 
    {
        if (currentSpline != null) 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + currentSpline.GetNode(nodeIndex).GetDirection());
        }
    }
}
