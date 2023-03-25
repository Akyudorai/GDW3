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

public class PlayerController : Controller
{
    //[SerializeField] private PlayerInput playerInput = null;
    //public PlayerInput PlayerInput => playerInput;

    //[Header("Camera")]
    //public float f_MouseSensitivity = 1.0f;
    //public bool b_InvertMouse = false;
    //public Vector2 hCameraClamp = new Vector2(-60, 30);
    //public Vector2 FovRange = new Vector2(60, 90);
    //public Vector2 v_Rotation;

    //[Header("Motion")]
    //public float CurrentSpeed = 0.0f;
    //public float QuickAcceleration = 10.0f;
    //public float TopAcceleration = 0.5f;
    //public float QuickMaxSpeed = 10.0f;
    //public float TopMaxSpeed = 20.0f;
    //public float BrakeSpeed = 0.1f;
    //public float JumpForce = 1.0f;
    //public float MaxFallSpeed = 20.0f;
    //public bool IsJumping = false;    
    //public bool GroundCheck = true;
    //public float f_AirTime = 0.0f;  // Track how long the player remains airborne.
    //public bool b_LimitVelocity = true;
    //public bool IsWallJumping = false;
    //private Vector2 v_Movement;
    //public bool IsSliding = false;
    //public bool IsOverridingMovement = false;

    //[Header("Debugging")]
    //public bool DebugInteractionRadius = false;
    ////public Vector3 Velocity;    
    //public float VelocityMagnitude;
    //public float CurrentSpeedRatio;
    ////public Vector3 targetInteractableHitPoint;
    //public Vector3 targetDirection = Vector3.zero;    

    //[Header("New Velocity System")]
    //public Vector2 v_MotionInput = Vector2.zero;
    //public float f_Speed = 0;
    //public Vector3 v_HorizontalVelocity = Vector3.zero;
    //public Vector3 v_VerticalVelocity = Vector3.zero;
    //public float f_Gravity = 9.8f;
    //public float f_RotationSpeed = 4.0f;
    //public float f_HorizontalJumpBoost = 2.0f;
    //public float f_AirResistance = 1.0f;
    //[Range(0.0f, 1.0f)] public float f_AirControlAmount = 0.5f;
    //public bool b_ShiftPressed = false;
    //public bool b_JumpPressed = false;
    //public bool b_Grounded = false;
    ////public bool b_LedgeGrab = false;
    //public float f_JumpBoostPercentage = 0.2f;
    //public float f_RailBoostPercentage = 0.2f;
    ////public bool b_CanLedgeCancel = true;

    //public PlayerState e_State = PlayerState.Active;
    ////public GameObject anim_RootTracker;    

    //[Header("SFX")]
    //public FMODUnity.EventReference jumpSound;
    ////FMOD.Studio.EventInstance jumpSFXInstance;

    // ----------------------------------------------------------------------------
    // MONOBEHAVIOR
    // ----------------------------------------------------------------------------

    protected override void Awake()
    {
        base.Awake();        

        // Initialize InteractionHandler
        interactionHandler = GetComponent<InteractionHandler>();
        interactionHandler.Initialize(this);

        // Initialize ManeuverHandler
        maneuverHandler = GetComponent<ManeuverHandler>();
        maneuverHandler.Initialize(this, animator);

        // Initialize InputHandler
        inputHandler = GetComponent<InputHandler>();
        inputHandler.Initialize(this);

        // Add Input Delegates
        inputHandler.JumpCallback += Jump;       
    }

    protected override void Start() 
    {
        // Assign static LocalController variable        
        GameManager.GetInstance().pcRef = this;        
        Local = this;

        // Set Position when scene is loaded
        GameManager.GetInstance().RespawnPlayer(SpawnPointManager.currentSpawnIndex);        

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 

        //jumpSFXInstance = FMODUnity.RuntimeManager.CreateInstance(jumpSound);
    }

    protected override void Update() 
    {
        // Update Manager Code
        interactionHandler.Tick();
        maneuverHandler.Tick();

        // Process Controller Animations
        HandleAnimations();
        
        // Prevent input when paused
        if (e_State == PlayerState.Locked || GameManager.GetInstance().IsPaused) return;
        
        HandleCamera();
        if (!maneuverHandler.b_IsSplineControlled && !maneuverHandler.b_IsLedgeHandling) {
            HandleMotion();
        }                        
    }

    //private void Camera()
    //{
    //    // Dynamically adjust FoV based on current speed relative to maximum speed, [60 - 90 FoV]
    //    // When at 0 magnitude, FoV 60
    //    // When at MaxSpeed magnitude, FOV 90
    //    //float currentSpeedRatio = VelocityMagnitude / MaxSpeed;
    //    //float resultingFoV = FovRange.x + currentSpeedRatio * (FovRange.y - FovRange.x);           // Starting at 60, add on the ratio multiplied by the different between min and max FOV 
    //    //cam.fieldOfView = Mathf.Clamp(resultingFoV, FovRange.x, FovRange.y);

    //    // Get the mouse input for horizontal and vertical axis and store as a float variable
    //    float cameraX = v_Rotation.y * f_MouseSensitivity * ((b_InvertMouse) ? 1 : -1);
    //    float cameraY = v_Rotation.x * f_MouseSensitivity;

    //    // Rotate the camera relative to the mouse input
    //    camera_pivot.transform.Rotate(cameraX, cameraY, 0);

    //    // Set the local Euler Angles and clamp the angles to prevent weird camera movements        
    //    camera_pivot.transform.localEulerAngles = new Vector3(ClampAngle(camera_pivot.transform.localEulerAngles.x, hCameraClamp.x, hCameraClamp.y), camera_pivot.transform.localEulerAngles.y, 0);
    //}

    

    //private void Movement() 
    //{
    //    if (IsWallJumping) return;   
    //    if (IsSliding) return;   
    //    if (IsOverridingMovement) return;
    
    //}

    //public IEnumerator OverrideMovement(float duration) 
    //{
    //    IsOverridingMovement = true;
    //    yield return new WaitForSeconds(duration);
    //    IsOverridingMovement = false;
    //}

    //private Vector3 GetGroundNormal()
    //{
    //    int layerMask = 1 << 6; // Ground Layer
    //    if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.5f, layerMask))
    //    {
    //        return hit.normal;
    //    }

    //    return Vector3.zero;
    //}

    //public void Brake(float speed)
    //{
    //    // Set the motion vector to a brake force to that will slow down the velocity
    //    if (rigid.velocity.magnitude > 0) {
    //        Vector3 brakeVector = -rigid.velocity * speed;                
    //        rigid.velocity += brakeVector;
    //    } 
    //}

    //public IEnumerator WallJump() 
    //{
    //    IsJumping = true;
    //    IsWallJumping = true;
    //    yield return new WaitForSeconds(1.0f);
    //    IsJumping = false;
    //    IsWallJumping = false;
    //}

    //public IEnumerator DelayVelocityLimiter(float time) 
    //{
    //    b_LimitVelocity = false;
    //    yield return new WaitForSeconds(time);
    //    b_LimitVelocity = true;
    //}
}
