using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NetworkedPlayerController : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    // VARIABLES & COMPONENTS
    // ----------------------------------------------------------------------------

    [Header("Components")]
    public NetworkIdentity identity;
    public Camera cam;
    public GameObject camera_pivot;
    public Rigidbody rigid;
    public GameObject mesh;
    public Animator animator;

    [Header("Handlers")]
    public InputHandler inputHandler;
    public ManeuverHandler maneuverHandler;
    public InteractionHandler interactionHandler;
    public PlayerState e_State = PlayerState.Active;

    [Header("Camera")]
    public float f_MouseSensitivity = 1.0f;
    public bool b_InvertMouse = false; // TODO: Use the invert boolean from the SettingsData class
    public Vector2 vec_CameraClamp = new Vector2(-60, 30);

    [Header("Motion")]
    public Vector3 v_HorizontalVelocity = Vector3.zero;
    public float f_CurrentSpeed = 0.0f;
    public float f_QuickAccelerationForce = 10.0f;
    public float f_SlowAccelerationForce = 0.5f;
    public float f_AccelerationSpeed = 10.0f;
    public float f_TopSpeed = 20.0f;
    public float f_BrakeSpeed = 0.1f;
    public float f_RotationSpeed = 4.0f;

    [Header("Air Control")]
    public Vector3 v_VerticalVelocity = Vector3.zero;
    public float f_Gravity = 9.8f;  // TODO: Move to a global settings variable
    public float MaxFallSpeed = 20.0f; // TODO: Move to a global settings variable
    public float f_AirTime = 0.0f;  // Track how long the player remains airborne.
    public float f_AirResistance = 1.0f;
    [Range(0.0f, 1.0f)] public float f_AirControlAmount = 0.5f;
    public bool b_IsGrounded = false;

    [Header("Jumping")]
    public bool b_IsJumping = false;
    public float f_JumpForce = 1.0f;
    public float f_JumpBoostPercentage = 0.2f;

    [Header("Networked Variables")]
    public Vector3 currentPos;
    public Vector3 prevPos;
    public int interpolationFrameCount = 1;
    public int elapsedFrames = 0;    

    [Header("States")]
    bool b_CheckForGround = true;


    // ----------------------------------------------------------------------------
    // MONOBEHAVIOR
    // ----------------------------------------------------------------------------

    private void Awake()
    {        
        //identity = GetComponent<NetworkIdentity>();
        rigid = GetComponent<Rigidbody>();

        inputHandler.JumpCallback += Jump;

        interactionHandler = GetComponent<InteractionHandler>();
        interactionHandler.Initialize(this);


        maneuverHandler = GetComponent<ManeuverHandler>();
        maneuverHandler.Initialize(this, animator);
    }

    private void Start()
    {
        if (Client.IsLocalPlayer(identity))
        {
            Debug.Log("I am the local client");
            
            // Toggle Cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;            
        }

        else
        {
            // Hide the camera
            cam.enabled = false;
        }
    }

    private void Update()
    {
        // Broadcast the networked position and rotation to the server
        if (Client.IsLocalPlayer(identity))
        {
            ClientSend.PlayerMovement(transform.position, mesh.transform.rotation);
        }

        else 
        {
            LerpPosition();
        }
        

        interactionHandler.Tick();
        maneuverHandler.Tick();
        
        HandleAnimations();

        if (e_State == PlayerState.Locked) return;        

        HandleCamera();
        if (!maneuverHandler.b_IsSplineControlled && !maneuverHandler.b_IsLedgeHandling)
        {
            HandleMotion();
        }                           
    }

    private void FixedUpdate() 
    {
        if (maneuverHandler.b_IsLedgeHandling) return;

        HandleGroundDetection();                
    }

    // ----------------------------------------------------------------------------
    // PLAYER CONTROLLER
    // ----------------------------------------------------------------------------

    public void SetPosition(Vector3 newPos) 
    {
        prevPos = transform.position;
        currentPos = newPos;   
        
        //transform.position = newPos;
    }

    private void LerpPosition() 
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFrameCount;
        Vector3 interpolatedPosition = Vector3.Lerp(prevPos, currentPos, interpolationRatio);
        elapsedFrames = (elapsedFrames + 1) % (interpolationFrameCount + 1);

        transform.position = interpolatedPosition;
    }

    private void HandleCamera()
    {
        // Prevent other clients from calling this code.
        if (!Client.IsLocalPlayer(identity)) return;

        // Get the mouse input for horizontal and vertical axis and store as a float variable
        float cameraX = inputHandler.CameraInput.y * f_MouseSensitivity * ((b_InvertMouse) ? 1 : -1);
        float cameraY = inputHandler.CameraInput.x * f_MouseSensitivity;

        // Rotate the camera relative to the mouse input
        camera_pivot.transform.Rotate(cameraX, cameraY, 0);

        // Set the local Euler Angles and clamp the angles to prevent weird camera movements        
        camera_pivot.transform.localEulerAngles = new Vector3(ClampAngle(camera_pivot.transform.localEulerAngles.x, vec_CameraClamp.x, vec_CameraClamp.y), camera_pivot.transform.localEulerAngles.y, 0);
    }

    private void HandleMotion()
    {
        // Prevent other clients from calling this code.
        if (!Client.IsLocalPlayer(identity)) return;
        
        // Prepare a motion vector to be used to modify velocity
        Vector3 motionVector = Vector3.zero;

        if (inputHandler.MotionInput.y > 0.1f || inputHandler.MotionInput.y < -0.1f || inputHandler.MotionInput.x > 0.1f || inputHandler.MotionInput.x < -0.1f)
        {
            //======================================================
            // Handles the horizontal motion of the player.
            //======================================================

            // Get Current Acceleration Rate
            float acceleration = ((v_HorizontalVelocity.magnitude < f_AccelerationSpeed) ? f_QuickAccelerationForce : f_SlowAccelerationForce);

            // Get direction of motion
            Vector3 forwardMotion = camera_pivot.transform.forward * inputHandler.MotionInput.y;
            Vector3 rightMotion = camera_pivot.transform.right * inputHandler.MotionInput.x;
            motionVector = forwardMotion + rightMotion;
            motionVector.y = 0;

            motionVector *= ((b_IsGrounded) ? 1.0f : f_AirControlAmount);

            // Apply Velocity Change
            v_HorizontalVelocity += motionVector * acceleration * Time.fixedDeltaTime;
            v_HorizontalVelocity = Vector3.ClampMagnitude(v_HorizontalVelocity, f_TopSpeed);
            //CurrentSpeed = v_HorizontalVelocity.magnitude;

            // Slow midair
            if (!b_IsGrounded)
            {
                v_HorizontalVelocity += -v_HorizontalVelocity * f_AirResistance * Time.fixedDeltaTime;
            }

            //======================================================
            // Handles the rotation of the character
            //======================================================

            // Calculate between motion vector and current velocity
            float angle = Vector3.Angle(motionVector.normalized, v_HorizontalVelocity.normalized);

            // If the angle between current velocity vector and the intended direciton is greater than 90 degrees
            if (angle > 90 && v_HorizontalVelocity != Vector3.zero && b_IsGrounded)
            {
                // Apply a brake force to the character
                // Intended to simulate sharp turns (character needs to stop themselves before moving around a sharp corner)
                if (v_HorizontalVelocity.magnitude > 0.1f)
                {
                    Vector3 brakeVector = -v_HorizontalVelocity * f_BrakeSpeed;
                    v_HorizontalVelocity += brakeVector * Time.fixedDeltaTime;
                }
                else
                {
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
        }

        // Otherwise, if there is no input along the X or Z axis
        else
        {
            // Set the motion vector to a brake force to that will slow down the velocity
            if (v_HorizontalVelocity.magnitude > 0.1f)
            {
                Vector3 brakeVector = -v_HorizontalVelocity * f_BrakeSpeed;
                v_HorizontalVelocity += brakeVector * Time.fixedDeltaTime;
            }
            else
            {
                v_HorizontalVelocity = Vector3.zero;
            }

            if (f_CurrentSpeed > 0)
            {
                f_CurrentSpeed *= f_BrakeSpeed * Time.fixedDeltaTime;
            }
        }

        // Clamp Horizontal Velocity
        if (v_HorizontalVelocity.magnitude > f_TopSpeed)
        {
            v_HorizontalVelocity = Vector3.ClampMagnitude(v_HorizontalVelocity, f_TopSpeed);
        }

        // Get direction based on angle of ground
        Vector3 motion_result = v_HorizontalVelocity + v_VerticalVelocity;
        float ground_angle = GetGroundAngle();
        Vector3 angled_motion = Quaternion.AngleAxis(ground_angle, mesh.transform.right) * motion_result;

        // Move the players position in the direction of velocity
        rigid.velocity = angled_motion;
        //UI_Manager.GetInstance().UpdateSpeedTracker(v_HorizontalVelocity.magnitude);

    }    

    private void Jump()
    {
        if (!Client.IsLocalPlayer(identity)) return;

        // If on a spline and jump button is released, detatch from it
        if (maneuverHandler.splineController.currentSpline != null)
        {
            maneuverHandler.splineController.Detatch();
            EventManager.OnPlayerJump?.Invoke();
            return;
        }

        // If grounded and not in the middle of a jump, then perform a jump.
        if (!b_IsJumping && b_IsGrounded)
        {
            StartCoroutine(maneuverHandler.DelayWallRunAllowance());

            // =========================== VERTICAL FORCE COMPONENT ================================

            Vector3 surfaceNormal = Vector3.up;

            // used for old slide mechanic
            //surfaceNormal = GetGroundNormal();

            Vector3 result = surfaceNormal;
            v_VerticalVelocity = Vector3.zero;
            ApplyForce(result * f_JumpForce);

            // =========================== HORIZONTAL FORCE COMPONENT ================================

            Vector3 jBoost = v_HorizontalVelocity;
            jBoost.y = 0;

            // Apply a boost to jumps to make it feel less slow amd simulate bunny hopping strategies
            ApplyForce(jBoost * f_JumpBoostPercentage);


            // =========================== INVOKE JUMP EVENT ================================
            b_IsGrounded = false;
            transform.SetParent(null);
            StartCoroutine(JumpDelay(0.5f));
            EventManager.OnPlayerJump?.Invoke();

            // Play Jump SFX
            FMOD.Studio.EventInstance jumpSFXInstance = SoundManager.CreateSoundInstance(SoundFile.Jump);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(jumpSFXInstance, GetComponent<Transform>(), rigid);
            jumpSFXInstance.start();
            jumpSFXInstance.release();
        }
    }

    private void HandleAnimations()
    {
        animator.SetFloat("Movement", v_HorizontalVelocity.magnitude);
        animator.SetBool("IsGrounded", b_IsGrounded);
    }

    private void HandleGroundDetection()
    {
        if (b_CheckForGround)
        {
            bool checkResult = IsGrounded();

            if (!b_IsGrounded && checkResult)
            {
                EventManager.OnPlayerLanding?.Invoke();
            }

            b_IsGrounded = checkResult;

            if (b_IsGrounded)
            {
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

        if (!b_IsGrounded)
        {
            f_AirTime += Time.fixedDeltaTime;
        }
    }

    public void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
    {
        //Debug.Log("Launched with force of " + force);
        v_HorizontalVelocity += new Vector3(force.x, 0, force.z);
        v_VerticalVelocity += new Vector3(0, force.y, 0);
        //rigid.AddForce(force, mode);
    }

    // To be used for negative camera angles and camera angles exceeding 180 degrees
    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    private bool IsGrounded()
    {
        int layerMask = 1 << 6;  // Ground Layer

        Vector3 boxCenter = rigid.GetComponent<Collider>().bounds.center;
        Vector3 halfExtents = rigid.GetComponent<Collider>().bounds.extents;

        float groundColliderHeight = 0.025f;
        halfExtents.y += groundColliderHeight;

        float maxDistance = GetComponent<Collider>().bounds.extents.y;

        bool rayResult = Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.1f, layerMask);
        if (rayResult) return rayResult;

        // // if the raycast returns false, attempt a box cast for ledge cases.
        /// BOX CAST SEEMS TO CAUSE THE SINKING EFFECT BUG, NEED AN ALTERNATIVE FOR GROUND CHECKS
        /// DOESNT EVEN FIX THE LEDGE DETECTION, JUST DELAYS IT
        bool boxResult = Physics.BoxCast(boxCenter, halfExtents / 2, Vector3.down, transform.rotation, maxDistance, layerMask);
        return boxResult;
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

    public IEnumerator JumpDelay(float duration)
    {
        b_IsJumping = true;
        b_CheckForGround = false;
        yield return new WaitForSeconds(duration);
        b_CheckForGround = true;
        b_IsJumping = false;
    }
}
