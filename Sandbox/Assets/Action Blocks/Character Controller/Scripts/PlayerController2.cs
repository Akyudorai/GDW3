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
    public GameObject mesh;

    [Header("Camera")]
    public float f_MouseSensitivity = 1.0f;
    public bool b_InvertMouse = false;
    public Vector2 hCameraClamp = new Vector2(-60, 30);
    public Vector2 FovRange = new Vector2(60, 90);
    public Vector2 v_Rotation;

    [Header("Motion")]
    public float CurrentSpeed = 0.0f;
    public float QuickAcceleration = 10.0f;
    public float TopAcceleration = 0.5f;
    public float QuickMaxSpeed = 10.0f;
    public float TopMaxSpeed = 20.0f;
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
    public bool IsSliding = false;
    public bool IsOverridingMovement = false;

    [Header("Interactions")]
    public float InteractionDistance = 5.0f;
    public Interactable targetInteractable;
     
    [Header("Splines")]
    public SplineController splineController;

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

        splineController.pcRef = this;
        splineController.rigid = rigid;
        splineController.mesh = mesh;

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

    private void Start() 
    {
        //GameManager.GetInstance().PlayerRef = this;
    }

    private void Update() 
    {
        if (splineController.currentSpline != null) 
        {
            float splineSpeed = Mathf.Min(QuickMaxSpeed, CurrentSpeed);
            splineController.SetTraversalSpeed(splineSpeed);      
        } 
        else 
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

                Vector3 normalDir = hit.normal;
                Vector3 upDir = Vector3.up;

                float angleBetween = Vector3.Angle(upDir, normalDir);

                if (angleBetween > 30.0f) {
                    IsSliding = true;                    
                } else {
                    IsSliding = false;
                }

                // Temporarily disabled until scale issue is resolved when parenting
                //transform.SetParent(hit.collider.gameObject.transform);

            } else {
                IsGrounded = false;
                IsSliding = false;  

                // Temporarily disabled until scale issue is resolved when parenting
                //if (transform.parent != null) {
                //    transform.parent = null;
                //}           
            }
        }
        

        if (!IsGrounded) {
            f_AirTime += Time.deltaTime;
        }                
    }

    private void Slide(Vector3 direction) 
    {   
        float slideSpeed = Mathf.Min(QuickMaxSpeed, CurrentSpeed);
        rigid.MovePosition(transform.position + direction * slideSpeed * Time.deltaTime);
    }

    private void Movement() 
    {
        if (IsWallJumping) return;   
        if (IsSliding) return;   
        if (IsOverridingMovement) return;
          

        // Prepare a motion vector to used to modify the velocity
        Vector3 motionVector = Vector3.zero;        

        // If there is input along the X or Z axis in either direction
        if (v_Movement.y > 0.1f || v_Movement.x > 0.1f || v_Movement.y < -0.1f || v_Movement.x < -0.1f) 
        {                     
            
            //======================================================
            // Handles the horizontal motion of the player.
            //======================================================

            // Get Current Acceleration Rate
            float acceleration = ((Velocity.magnitude < QuickMaxSpeed) ? QuickAcceleration : TopAcceleration);

            // Get direction of motion
            Vector3 forwardMotion = camera_pivot.transform.forward * v_Movement.y;               
            Vector3 rightMotion = camera_pivot.transform.right * v_Movement.x;
            motionVector = forwardMotion + rightMotion;
            motionVector.y = 0;

            // Apply Velocity Change
            Velocity += motionVector * acceleration * Time.deltaTime;
            Velocity = Vector3.ClampMagnitude(Velocity, TopMaxSpeed);
            CurrentSpeed = Velocity.magnitude;
            
            //======================================================

            //======================================================
            // Handles the rotation of the character
            //======================================================

            // Calculate between motion vector and current velocity
            float angle = Vector3.Angle(motionVector.normalized, Velocity.normalized);

            // If the angle between current velocity vector and the intended direciton is greater than 90 degrees
            if (angle > 90 && Velocity != Vector3.zero) 
            {
                // Apply a brake force to the character
                // Intended to simulate sharp turns (character needs to stop themselves before moving around a sharp corner)
                if (Velocity.magnitude > 0.1f) {
                    Vector3 brakeVector = -Velocity * BrakeSpeed;
                    Velocity += brakeVector * Time.deltaTime;            
                } else {
                    Velocity = Vector3.zero;
                }                
            } 
            
            // Otherwise, if the angle between current velocity vector and intended direction is less than 90 degrees
            else 
            {
                // Grab the cross product to determine which way we want to rotate
                Vector3 delta = ((transform.position + motionVector) - transform.position).normalized;
                Vector3 cross = Vector3.Cross(delta, Velocity.normalized);

                // motionVector is Parallel with Velocity; do nothing
                if (cross == Vector3.zero) { }
                
                // motionVector is to the left of velocity direction
                else if (cross.y > 0) 
                {    
                    // Rotate the Direction of Velocity by a negative angle                    
                    Vector3 newDirection = Quaternion.AngleAxis(-angle * Time.deltaTime, Vector3.up) * Velocity;
                    Velocity = newDirection;
                } 

                // motionVector is to the right of the velocity direction
                else 
                {
                    // Rotate Direction of Velocity by a positive angle
                    Vector3 newDirection = Quaternion.AngleAxis(angle * Time.deltaTime, Vector3.up) * Velocity;
                    Velocity = newDirection;
                }
            }

            // Lastly, rotate the character to look towards the direction of velocity
            /// We can have the players head rotate to look towards intended motion, while body faces direction of velocity in future
            mesh.transform.LookAt(mesh.transform.position + Velocity);

            //======================================================
        } 
        
        // Otherwise, if there is no input along the X or Z axis
        else 
        {   
            // Set the motion vector to a brake force to that will slow down the velocity
            if (Velocity.magnitude > 0.1f) {
                Vector3 brakeVector = -Velocity * BrakeSpeed;
                Velocity += brakeVector * Time.deltaTime;            
            } else {
                Velocity = Vector3.zero;
            }
            
            if (CurrentSpeed > 0) {
                CurrentSpeed *= BrakeSpeed * Time.deltaTime;
            } 
        }                                                  
        
        // Move the players position in the direction of velocity
        rigid.MovePosition(transform.position + Velocity * Time.deltaTime);
        
        //rigid.velocity += motionVector * CurrentSpeed * Time.deltaTime;

        //rigid.velocity += ((IsGrounded) ? motionVector : motionVector * 0.6f); // Change 0.25 to whatever value you want air motion control ratio to be
        

        //Vector3 xzVel = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        //Vector3 yVel = new Vector3(0, rigid.velocity.y, 0);

        //xzVel = Vector3.ClampMagnitude(xzVel, TopMaxSpeed);
        //yVel = Vector3.ClampMagnitude(yVel, MaxFallSpeed);        
        //rigid.MovePosition(transform.position + (xzVel + yVel) * Time.deltaTime);    
        //if (b_LimitVelocity) 
            //rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -TopMaxSpeed, TopMaxSpeed), rigid.velocity.y, Mathf.Clamp(rigid.velocity.z, -TopMaxSpeed, TopMaxSpeed));            

        
        //targetDirection.x = xzVel.x;
        //targetDirection.z = xzVel.z;        
        //targetDirection.Normalize();
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

    public IEnumerator OverrideMovement(float duration) 
    {
        IsOverridingMovement = true;
        yield return new WaitForSeconds(duration);
        IsOverridingMovement = false;
    }

    private void Jump()
    {
        // If on a spline, detatch from it
        if (splineController.currentSpline != null) {
            splineController.Detatch();
            return;
        }        

        // If grounded and not in the middle of a jump, then perform a jump.
        if (!IsJumping && IsGrounded) 
        {
            Vector3 surfaceNormal = Vector3.up;
            int layerMask = 1 << 6; // Ground Layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.5f, layerMask))
            {
                surfaceNormal = hit.normal;                                
            }

            Vector3 result = surfaceNormal;
            if (IsSliding) result += Vector3.up;
            ApplyForce(result * ((IsSliding) ? JumpForce * 2.0f : JumpForce));
            IsGrounded = false;

            Debug.Log("Jump");
            StartCoroutine(JumpDelay());
        }       
    }

    public void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
    {        
        //Debug.Log("Launched with force of " + force);
        rigid.AddForce(force, mode);
    } 

    public void Brake(float speed)
    {
        // Set the motion vector to a brake force to that will slow down the velocity
        if (rigid.velocity.magnitude > 0) {
            Vector3 brakeVector = -rigid.velocity * speed;                
            rigid.velocity += brakeVector;
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
                if (targetInteractable != null) // BUG: for some reason, it doesn't often detect there's a target interactable there
                {
                    if (targetInteractable.gameObject == hit.gameObject) 
                    {
                        targetInteractableHitPoint = hit.ClosestPoint(transform.position);    
                        if (Physics.Raycast(transform.position, targetInteractableHitPoint - transform.position, out RaycastHit hitResult))
                        {
                            targetInteractable.Interact(this, hitResult);
                        }                
                    }
                }
                
            }
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

    private void OnCollisionEnter(Collision col) 
    {
        // if (col.gameObject.tag == "Interactable" && col.gameObject.GetComponent<WallInteractable>() != null && currentSpline == null) 
        // {   
        //     targetInteractable = col.gameObject.GetComponent<Interactable>();                 
        //     targetInteractableHitPoint = col.contacts[0].point;

        //     if (targetInteractable != null) // BUG: for some reason, it doesn't often detect there's a target interactable there
        //     {
        //         if (targetInteractable.gameObject == col.gameObject) 
        //         {  
        //             if (Physics.Raycast(transform.position, targetInteractableHitPoint - transform.position, out RaycastHit hitResult))
        //             {
        //                 targetInteractable.Interact(this, hitResult);
        //             }                
        //         }
        //     }            
        // }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;

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
