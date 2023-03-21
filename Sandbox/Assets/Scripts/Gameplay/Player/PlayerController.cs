using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState 
{
    Active,
    Locked,
    Paused
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController LocalPlayer;

    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [Header("Components")]
    public Camera cam;
    public GameObject camera_pivot;
    public Rigidbody rigid;
    public Collider capsuleCollider;
    public GameObject mesh;
    public Animator animator;

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
    public bool GroundCheck = true;
    public float f_AirTime = 0.0f;  // Track how long the player remains airborne.
    public bool b_LimitVelocity = true;
    public bool IsWallJumping = false;
    private Vector2 v_Movement;
    public bool IsSliding = false;
    public bool IsOverridingMovement = false;

    [Header("Interactions")]
    public InteractionHandler interactionHandler;
    public ManeuverHandler maneuverHandler;
    //public SplineController splineController;

    // NOTE TO SELF: 
    /// Current tracks every wall that has been ran on and counts down a timer. Until it can be ran on again 
    /// Considering the idea where instead of tracking every single wall, we just track the most recent wall ran on.
    /// This would allow us to jump between two walls infinitely for extended "hallway" sequences, as the previous wall timer
    /// would get reset to zero once the new wall timer is set.
    /// Alternatively, could keep it as it is now, and add a function where everytime we run on a new wall, we reduce the timer of 
    /// all other timers by X seconds.  Would still encourage chaining jumps between walls, but also still allow us to restrict how 
    /// quickly they can do so. 
    //public Dictionary<WallInteractable, float> wallDelays = new Dictionary<WallInteractable, float>();

    [Header("Debugging")]
    public bool DebugInteractionRadius = false;
    //public Vector3 Velocity;    
    public float VelocityMagnitude;
    public float CurrentSpeedRatio;
    //public Vector3 targetInteractableHitPoint;
    public Vector3 targetDirection = Vector3.zero;    

    [Header("New Velocity System")]
    public Vector2 v_MotionInput = Vector2.zero;
    public float f_Speed = 0;
    public Vector3 v_HorizontalVelocity = Vector3.zero;
    public Vector3 v_VerticalVelocity = Vector3.zero;
    public float f_Gravity = 9.8f;
    public float f_RotationSpeed = 4.0f;
    public float f_HorizontalJumpBoost = 2.0f;
    public float f_AirResistance = 1.0f;
    [Range(0.0f, 1.0f)] public float f_AirControlAmount = 0.5f;
    public bool b_ShiftPressed = false;
    public bool b_JumpPressed = false;
    public bool b_Grounded = false;
    //public bool b_LedgeGrab = false;
    public float f_JumpBoostPercentage = 0.2f;
    public float f_RailBoostPercentage = 0.2f;
    //public bool b_CanLedgeCancel = true;

    public PlayerState e_State = PlayerState.Active;
    //public GameObject anim_RootTracker;    
    public ParticleSystem ps_MaxSpeed;

    [Header("SFX")]
    public FMODUnity.EventReference jumpSound;
    //FMOD.Studio.EventInstance jumpSFXInstance;

    private void Awake() 
    {        
        rigid = GetComponent<Rigidbody>();    
        interactionHandler = GetComponent<InteractionHandler>();
        interactionHandler.Initialize(this);

        maneuverHandler = GetComponent<ManeuverHandler>();          
        maneuverHandler.Initialize(this, animator);

        //splineController.pcRef = this;
        //splineController.rigid = rigid;
        //splineController.mesh = mesh;        
    }

    private void Start() 
    {
        // Set Reference when scene is loaded
        GameManager.GetInstance().pcRef = this;
        LocalPlayer = this;

        // Set Position when scene is loaded
        GameManager.GetInstance().RespawnPlayer(SpawnPointManager.currentSpawnIndex);        

        //GameManager.GetInstance().PlayerRef = this;       
        InputManager.GetInput().Player.Shift.performed += ShiftWithContext;
        InputManager.GetInput().Player.Shift.canceled += ShiftCancelWithContext;
        InputManager.GetInput().Player.Escape.performed += cntxt => UI_Manager.GetInstance().TogglePhonePanel(!UI_Manager.GetInstance().PhonePanel.activeInHierarchy);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 

        //jumpSFXInstance = FMODUnity.RuntimeManager.CreateInstance(jumpSound);
    }

    private void OnDestroy() 
    {
        if (InputManager.GetInput() == null) return;

        InputManager.GetInput().Player.Shift.performed -= ShiftWithContext;
        InputManager.GetInput().Player.Shift.canceled -= ShiftCancelWithContext;
    }

    public void MoveCtx(InputAction.CallbackContext ctx) 
    {
        if (GameManager.GetInstance().IsPaused) 
        {
            v_MotionInput = Vector2.zero;
            return;
        }

        var inputValue = ctx.ReadValue<Vector2>();
        v_MotionInput = inputValue;
    }

    public void JumpCtx(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }

        Jump();
    }

    public void LookCtx(InputAction.CallbackContext ctx) 
    {
        if (GameManager.GetInstance().IsPaused) 
        {
            v_Rotation = Vector2.zero;
            return;
        }

        var inputValue = ctx.ReadValue<Vector2>();
        v_Rotation = inputValue;
    }

    public void InteractCtx(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }
        interactionHandler.Interact(this, InteractionType.Social);
        interactionHandler.Interact(this, InteractionType.VendingMachine);
    }

    public void WallRunCtx(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }
        interactionHandler.Interact(this, InteractionType.Wall);
    }

    public void ZiplineCtx(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }
        interactionHandler.Interact(this, InteractionType.Zipline);
    }

    public void RailGrindCtx(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }
        interactionHandler.Interact(this, InteractionType.Rail);
    }

    public void LedgeGrabCtx(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }
        interactionHandler.Interact(this, InteractionType.Ledge);
    }

    public void LedgeClimbCtx(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }
        maneuverHandler.PerformLedgeClimb();
    }

    public void LedgeReleaseCtx(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed || GameManager.GetInstance().IsPaused) { return; }
        maneuverHandler.PerformLedgeDrop();
    }

    
    private void ShiftWithContext(InputAction.CallbackContext context) 
    {
        //b_ShiftPressed = true;
    }

    private void ShiftCancelWithContext(InputAction.CallbackContext context) 
    {
        //b_ShiftPressed = false;
    }    

    public void SetPlayerState(PlayerState state)
    {
        e_State = state;
    }

    private void Update() 
    {
        GetGroundAngle();

        if (v_HorizontalVelocity.magnitude > TopMaxSpeed) {
            v_HorizontalVelocity = Vector3.ClampMagnitude(v_HorizontalVelocity, TopMaxSpeed);
        }

        animator.SetFloat("Movement", v_HorizontalVelocity.magnitude);
        animator.SetBool("IsGrounded", b_Grounded);        

        bool maxSpeed = rigid.velocity.magnitude > QuickMaxSpeed;
        if (maxSpeed && !ps_MaxSpeed.isPlaying) {
            ps_MaxSpeed.Play();
        } else if (!maxSpeed && ps_MaxSpeed.isPlaying) {
            ps_MaxSpeed.Stop();
        }

        interactionHandler.Tick();
        maneuverHandler.Tick();

        if (!maneuverHandler.b_IsSplineControlled && !maneuverHandler.b_IsLedgeHandling) {
            Movement();
        }

        if (GameManager.GetInstance().IsPaused) return;
        if (e_State == PlayerState.Locked) return;        

        Camera();
        
        if (!b_Grounded) {
            f_AirTime += Time.fixedDeltaTime;
        }         
    }

    private void FixedUpdate() 
    {
        //Debug.Log(IsSliding);
        if (maneuverHandler.b_IsLedgeHandling)
        {
            return;
        } 

        // Ground Check
        if (GroundCheck) 
        {     
            bool checkResult = IsGrounded();
            
            if (!b_Grounded && checkResult) {
                EventManager.OnPlayerLanding?.Invoke();            
            } 

            b_Grounded = checkResult;

            if (b_Grounded) {                
                f_AirTime = 0.0f;
                v_VerticalVelocity = Vector3.zero;                

                // Handle Sliding Maneuver
                // SlideCheck();

                // Temporarily disabled until scale issue is resolved when parenting
                //transform.SetParent(hit.collider.gameObject.transform);
            }
    
            else 
            {                
                //Debug.Log("Applying Gravity!");
                //IsSliding = false;  
                v_VerticalVelocity -= Vector3.up * f_Gravity * Time.fixedDeltaTime;
                v_VerticalVelocity = Vector3.ClampMagnitude(v_VerticalVelocity, MaxFallSpeed);                

                if (transform.parent != null)
                {
                    transform.SetParent(null);
                }
                // Temporarily disabled until scale issue is resolved when parenting
                //if (transform.parent != null) {
                //    transform.parent = null;
                //}  
            }
        }
    }

    private void Movement() 
    {
        if (IsWallJumping) return;   
        if (IsSliding) return;   
        if (IsOverridingMovement) return;

        // rigid.velocity = v_DirectionalVelocity;        

        // Prepare a motion vector to used to modify the velocity
        Vector3 motionVector = Vector3.zero;        

        // If there is input along the X or Z axis in either direction
        if (v_MotionInput.y > 0.1f || v_MotionInput.x > 0.1f || v_MotionInput.y < -0.1f || v_MotionInput.x < -0.1f) 
        {                              
            //======================================================
            // Handles the horizontal motion of the player.
            //======================================================

            // Get Current Acceleration Rate
            float acceleration = ((v_HorizontalVelocity.magnitude < QuickMaxSpeed) ? QuickAcceleration : TopAcceleration);

            // Get direction of motion
            Vector3 forwardMotion = camera_pivot.transform.forward * v_MotionInput.y;               
            Vector3 rightMotion = camera_pivot.transform.right * v_MotionInput.x;
            motionVector = forwardMotion + rightMotion;
            motionVector.y = 0;

            motionVector *= ((b_Grounded) ? 1.0f : f_AirControlAmount);

            // Apply Velocity Change
            v_HorizontalVelocity += motionVector * acceleration * Time.fixedDeltaTime;
            v_HorizontalVelocity = Vector3.ClampMagnitude(v_HorizontalVelocity, (b_ShiftPressed) ? TopMaxSpeed / 4: TopMaxSpeed);
            //CurrentSpeed = v_HorizontalVelocity.magnitude;

            // Slow midair
            if (!b_Grounded) {
                v_HorizontalVelocity += -v_HorizontalVelocity * f_AirResistance * Time.fixedDeltaTime;
            }
            
            //======================================================

            //======================================================
            // Handles the rotation of the character
            //======================================================

            // Calculate between motion vector and current velocity
            float angle = Vector3.Angle(motionVector.normalized, v_HorizontalVelocity.normalized);

            // If the angle between current velocity vector and the intended direciton is greater than 90 degrees
            if (angle > 90 && v_HorizontalVelocity != Vector3.zero && b_Grounded) 
            {
                // Apply a brake force to the character
                // Intended to simulate sharp turns (character needs to stop themselves before moving around a sharp corner)
                if (v_HorizontalVelocity.magnitude > 0.1f) {
                    Vector3 brakeVector = -v_HorizontalVelocity * BrakeSpeed;
                    v_HorizontalVelocity += brakeVector * Time.fixedDeltaTime;            
                } else {
                    v_HorizontalVelocity = Vector3.zero;
                }                
            } 
            
            // Otherwise, if the angle between current velocity vector and intended direction is less than 90 degrees
            else 
            {
                // Grab the cross product to determine which way we want to rotate
                Vector3 delta = ((transform.position + motionVector) - transform.position).normalized;
                Vector3 cross = Vector3.Cross(delta, v_HorizontalVelocity.normalized);

                // motionVector is Parallel with Velocity; do nothing
                if (cross == Vector3.zero) { }
                
                // motionVector is to the left of velocity direction
                else if (cross.y > 0) 
                {    
                    // Rotate the Direction of Velocity by a negative angle                    
                    Vector3 newDirection = Quaternion.AngleAxis(-angle * f_RotationSpeed * Time.fixedDeltaTime, Vector3.up) * v_HorizontalVelocity; 
                    v_HorizontalVelocity = newDirection;
                } 

                // motionVector is to the right of the velocity direction
                else 
                {
                    // Rotate Direction of Velocity by a positive angle
                    Vector3 newDirection = Quaternion.AngleAxis(angle * f_RotationSpeed * Time.fixedDeltaTime, Vector3.up) * v_HorizontalVelocity;
                    v_HorizontalVelocity = newDirection;
                }
            }

            // Lastly, rotate the character to look towards the direction of velocity
            /// We can have the players head rotate to look towards intended motion, while body faces direction of velocity in future
            mesh.transform.LookAt(mesh.transform.position + v_HorizontalVelocity);

            //======================================================
        } 
        
        // Otherwise, if there is no input along the X or Z axis
        else 
        {   
            // Set the motion vector to a brake force to that will slow down the velocity
            if (v_HorizontalVelocity.magnitude > 0.1f) {
                Vector3 brakeVector = -v_HorizontalVelocity * BrakeSpeed;
                v_HorizontalVelocity += brakeVector * Time.fixedDeltaTime;            
            } else {
                v_HorizontalVelocity = Vector3.zero;
            }
            
            if (CurrentSpeed > 0) {
                CurrentSpeed *= BrakeSpeed * Time.fixedDeltaTime;
            } 
        }

        // Get direction based on angle of ground
        Vector3 motion_result = v_HorizontalVelocity + v_VerticalVelocity;
        float ground_angle = GetGroundAngle();
        Vector3 angled_motion = Quaternion.AngleAxis(ground_angle, mesh.transform.right) * motion_result;    
 
        // Move the players position in the direction of velocity
        rigid.velocity = angled_motion;     
        UI_Manager.GetInstance().UpdateSpeedTracker(v_HorizontalVelocity.magnitude);   
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

    private Vector3 GetGroundNormal()
    {
        int layerMask = 1 << 6; // Ground Layer
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.5f, layerMask))
        {
            return hit.normal;
        }

        return Vector3.zero;
    }

    private float GetGroundAngle() 
    {
        int layerMask = 1 << 6; // Ground Layer
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.5f, layerMask))
        {
            Vector3 normal = hit.normal;
            Vector3 up = Vector3.up;

            Vector3 forward = mesh.transform.forward;               
            float angleBetween = 90.0f - Vector3.Angle(forward, normal);
            return angleBetween;
        }

        return 0;       
    }

    private void Jump()
    {               
        // If on a spline and jump button is released, detatch from it
        if (maneuverHandler.splineController.currentSpline != null) {
            maneuverHandler.splineController.Detatch();            
            EventManager.OnPlayerJump?.Invoke();
            return;
        } 

        // If grounded and not in the middle of a jump, then perform a jump.
        if (!IsJumping && b_Grounded) 
        {
            StartCoroutine(maneuverHandler.DelayWallRunAllowance());

            // =========================== VERTICAL FORCE COMPONENT ================================

            Vector3 surfaceNormal = Vector3.up;            
            
            // used for old slide mechanic
            //surfaceNormal = GetGroundNormal();

            Vector3 result = surfaceNormal;
            v_VerticalVelocity = Vector3.zero;
            ApplyForce(result * JumpForce);

            // =========================== HORIZONTAL FORCE COMPONENT ================================

            Vector3 jBoost = v_HorizontalVelocity;
            jBoost.y = 0;

            // Apply a boost to jumps to make it feel less slow amd simulate bunny hopping strategies
            ApplyForce(jBoost * f_JumpBoostPercentage);
            

            // =========================== INVOKE JUMP EVENT ================================
            b_Grounded = false;
            transform.SetParent(null);
            StartCoroutine(JumpDelay());
            EventManager.OnPlayerJump?.Invoke();

            // Play Jump SFX
            FMOD.Studio.EventInstance jumpSFXInstance = SoundManager.CreateSoundInstance(SoundFile.Jump);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(jumpSFXInstance, GetComponent<Transform>(), rigid);
            jumpSFXInstance.start();
            jumpSFXInstance.release();
        }            
    }

    public void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
    {        
        //Debug.Log("Launched with force of " + force);
        v_HorizontalVelocity += new Vector3(force.x, 0, force.z);
        v_VerticalVelocity += new Vector3(0, force.y, 0);
        //rigid.AddForce(force, mode);
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

    public bool IsGrounded()
    {
        int layerMask = 1 << 6; // Ground Layer
           
        Vector3 boxCenter = rigid.GetComponent<Collider>().bounds.center;
        Vector3 halfExtents = rigid.GetComponent<Collider>().bounds.extents;

        float groundColliderHeight = 0.025f;
        halfExtents.y += groundColliderHeight;

        float maxDistance = GetComponent<Collider>().bounds.extents.y;

        // Calculate angle of ground


        
        bool rayResult = Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.1f, layerMask);        
        if (rayResult) return rayResult;
        
        // // if the raycast returns false, attempt a box cast for ledge cases.
        /// BOX CAST SEEMS TO CAUSE THE SINKING EFFECT BUG, NEED AN ALTERNATIVE FOR GROUND CHECKS
        /// DOESNT EVEN FIX THE LEDGE DETECTION, JUST DELAYS IT
        bool boxResult = Physics.BoxCast(boxCenter, halfExtents/2, Vector3.down, transform.rotation, maxDistance, layerMask);        
        return boxResult;        
    }

    private void OnCollisionEnter(Collision col) 
    {       
        //Debug.Log(col.gameObject);
        if (col.gameObject.tag == "Platform")
        {
            // Set Parent if result is of tag "Platform"
            transform.SetParent(col.gameObject.transform);  
        }
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

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Collectible") 
        {
            Collectible c = other.GetComponent<Collectible>();
            c.OnCollect();

            // Play Collectible SFX
            FMOD.Studio.EventInstance collectibleSFX = SoundManager.CreateSoundInstance(SoundFile.CollectiblePickup);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(collectibleSFX, gameObject.transform, rigid);
            collectibleSFX.start();
            collectibleSFX.release(); 
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;

        if (!b_Grounded) {
            Gizmos.DrawLine(transform.position + Vector3.up*0.1f, transform.position - Vector3.up * 0.5f);
        }
    }
}
