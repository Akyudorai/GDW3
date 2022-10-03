using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [Header("Components")]
    public Camera cam;
    public GameObject camera_pivot;
    public Rigidbody rigid;    
    public InputActions inputAction;

    [Header("Camera")]
    public float f_MouseSensitivity = 1.0f;
    public bool b_InvertMouse = false;
    public Vector2 hCameraClamp = new Vector2(-60, 30);
    public Vector2 FovRange = new Vector2(60, 90);
    public Vector2 v_Rotation;

    [Header("Motion")]
    public float RunSpeed = 5.0f;
    public float MaxSpeed = 10.0f;
    public float BrakeSpeed = 0.1f;
    public float JumpForce = 1.0f;
    public float MaxFallSpeed = 20.0f;
    public bool IsJumping = false;
    public bool IsGrounded = false;
    public bool GroundCheck = true;
    public float f_AirTime = 0.0f;  // Track how long the player remains airborne.
    public bool b_LimitVelocity = true;
    public bool IsWallJumping = false;
    private Vector2 v_Movement;


    [Header("Interactions")]
    public float InteractionDistance = 5.0f;
    public Interactable targetInteractable;
     
    [Header("Splines")]
    public SplinePath currentSpline = null;

    [Header("Debugging")]
    public bool DebugInteractionRadius = false;
    public Vector3 Velocity;    
    public float VelocityMagnitude;
    public float CurrentSpeedRatio;
    public Vector3 targetInteractableHitPoint;
    public Vector3 targetDirection = Vector3.zero;    
    

    private void Awake() 
    {
        inputAction = new InputActions();
        inputAction.Player.Move.performed += cntxt => v_Movement = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => v_Movement = Vector2.zero;
        inputAction.Player.Look.performed += cntxt => v_Rotation = cntxt.ReadValue<Vector2>();
        inputAction.Player.Look.canceled += cntxt => v_Rotation = Vector2.zero;
        inputAction.Player.Jump.performed += cntxt => Jump();
        inputAction.Player.Interact.performed += cntxt => Interact();

        rigid = GetComponent<Rigidbody>();        

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() 
    {
        inputAction.Player.Enable();
    }

    private void OnDisable() 
    {
        inputAction.Player.Disable();
    }

    private void Update() 
    {
        if (currentSpline != null) 
        {
            transform.position = currentSpline.GetCurrentNode().GetCurrentPosition();
            currentSpline.GetCurrentNode().Traverse(1.0f);                       
        } else 
        {
            Movement();
        }

        Camera();

         // Ground Check
        if (GroundCheck) 
        {
            int layerMask = 1 << 6; // Ground Layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.5f, layerMask))
            {
                IsGrounded = true;                                            
                f_AirTime = 0.0f;            
            } else {
                IsGrounded = false;
            }
        }
        

        if (!IsGrounded) {
            f_AirTime += Time.deltaTime;
        }                
    }

    private void Movement() 
    {
        if (IsWallJumping) return;

        // Prepare a motion vector to used to modify the velocity
        Vector3 motionVector = Vector3.zero;        

        // If there is input along the X or Z axis in either direction
        if (v_Movement.y > 0.1f || v_Movement.x > 0.1f || v_Movement.y < -0.1f || v_Movement.x < -0.1f) 
        {            
            // Set the motion vector to be relative to the input direction multiplied by the player speed and DeltaTime
            //motionVector = new Vector3(horizontal * RunSpeed * Time.deltaTime, 0, vertical * RunSpeed * Time.deltaTime);
            Vector3 forwardMotion = camera_pivot.transform.forward * v_Movement.y * RunSpeed * Time.deltaTime;
            Vector3 rightMotion = camera_pivot.transform.right * v_Movement.x * RunSpeed * Time.deltaTime;
            motionVector = forwardMotion + rightMotion;
            motionVector.y = 0;
        } 
        
        // Otherwise, if there is no input along the X or Z axis
        else 
        {
            // Set the motion vector to a brake force to that will slow down the velocity
            if (rigid.velocity.magnitude > 0 || rigid.velocity.magnitude < 0) {
                Vector3 brakeVector = -rigid.velocity * BrakeSpeed;                
                motionVector = brakeVector;
                motionVector.y = 0;
            } 
        }                                          

        // Lastly, set the new velocity for the rigidbody equal to our resulting velocity.        
        rigid.velocity += ((IsGrounded) ? motionVector : motionVector * 0.25f); // Change 0.25 to whatever value you want air motion control ratio to be

        Vector3 xzVel = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        Vector3 yVel = new Vector3(0, rigid.velocity.y, 0);

        xzVel = Vector3.ClampMagnitude(xzVel, MaxSpeed);
        yVel = Vector3.ClampMagnitude(yVel, MaxFallSpeed);        
        rigid.MovePosition(transform.position + (xzVel + yVel) * Time.deltaTime);    
        if (b_LimitVelocity) 
            rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -MaxSpeed, MaxSpeed), rigid.velocity.y, Mathf.Clamp(rigid.velocity.z, -MaxSpeed, MaxSpeed));    

        targetDirection.x = xzVel.x;
        targetDirection.z = xzVel.z;        
        targetDirection.Normalize();
    }

    private void Camera()
    {
        // Dynamically adjust FoV based on current speed relative to maximum speed, [60 - 90 FoV]
        // When at 0 magnitude, FoV 60
        // When at MaxSpeed magnitude, FOV 90
        //float currentSpeedRatio = VelocityMagnitude / MaxSpeed;
        //float resultingFoV = FovRange.x + currentSpeedRatio * (FovRange.y - FovRange.x);           // Starting at 60, add on the ratio multiplied by the different between min and max FOV 
        //cam.fieldOfView = Mathf.Clamp(resultingFoV, FovRange.x, FovRange.y);
        
        // Get the mouse input for horizontal and vertical axis and store as a float variable
        float cameraX = v_Rotation.y * f_MouseSensitivity * ((b_InvertMouse) ? 1 : -1);
        float cameraY = v_Rotation.x * f_MouseSensitivity;

        // Rotate the camera relative to the mouse input
        camera_pivot.transform.Rotate(cameraX, cameraY, 0);        

        // Set the local Euler Angles and clamp the angles to prevent weird camera movements        
        camera_pivot.transform.localEulerAngles = new Vector3(ClampAngle(camera_pivot.transform.localEulerAngles.x, hCameraClamp.x, hCameraClamp.y), camera_pivot.transform.localEulerAngles.y, 0);   
    }

    private void Jump()
    {
        // If on a spline, detatch from it
        if (currentSpline != null) {
            currentSpline.GetCurrentNode().Detatch();
            return;
        }

        // If grounded and not in the middle of a jump, then perform a jump.
        if (!IsJumping && IsGrounded) 
        {
            rigid.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);            
            IsGrounded = false;
            StartCoroutine(JumpDelay());
        }       
    }

    public IEnumerator JumpDelay() 
    {
        IsJumping = true;
        GroundCheck = false;
        yield return new WaitForSeconds(0.5f);
        GroundCheck = true;
        IsJumping = false;
    }

    private void Interact()
    {        
        // Run a spherical scan for all interactables within the players vicinity                
        /// Can move this to update if we want to scan constantly, not just when interact button is pressed
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.up * 1.5f, InteractionDistance);
        foreach (var hit in hitColliders) {
            if (hit.gameObject.tag == "Interactable") 
            {   
                // If no interactable has been set yet, set the first one we find
                if (targetInteractable == null) targetInteractable = hit.gameObject.GetComponent<Interactable>();
                else 
                {
                    // Get distance from player to interactable and compare with current target interactable
                    float dist_target = Vector3.Distance(transform.position, targetInteractable.gameObject.transform.position);
                    float dist_compare = Vector3.Distance(transform.position, hit.gameObject.transform.position);

                    // If the new target is closer than the old, replace it
                    if (dist_compare < dist_target) {
                        targetInteractable = hit.gameObject.GetComponent<Interactable>();                        
                    }
                }

                // Get the closest interactable point
                if (targetInteractable.gameObject == hit.gameObject) 
                {
                    targetInteractableHitPoint = hit.ClosestPoint(transform.position);                    
                }
            }
        }  
        

        // Interact with target interactable, if one exists
        if (targetInteractable != null && currentSpline == null) {                       
            targetInteractable.Interact(this);
        }

        if (Vector3.Distance(transform.position, targetInteractableHitPoint) > InteractionDistance) {
            targetInteractable = null;
        }
    }

    public IEnumerator WallJump() 
    {
        IsJumping = true;
        IsWallJumping = true;
        yield return new WaitForSeconds(1.0f);
        IsJumping = false;
        IsWallJumping = false;
    }

    public IEnumerator DelayVelocityLimiter(float time) 
    {
        b_LimitVelocity = false;
        yield return new WaitForSeconds(time);
        b_LimitVelocity = true;
    }

    // To be used for negative angles
    private float ClampAngle(float angle, float from, float to) 
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360+from);
        return Mathf.Min(angle, to);
    }

    private void OnDrawGizmos() 
    {
        if (DebugInteractionRadius) Gizmos.DrawWireSphere(transform.position + transform.up * 1.5f, InteractionDistance);             
    
        if (!IsGrounded) {
            Gizmos.DrawLine(transform.position, transform.position - Vector3.up * 0.5f);
        }

        if (targetInteractable != null) 
        {
            Gizmos.DrawLine(transform.position + transform.up * 1.5f, targetInteractableHitPoint);
        }
    }
}
