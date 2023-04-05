using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NetworkedPlayerController : Controller
{
    // ----------------------------------------------------------------------------
    // VARIABLES & COMPONENTS
    // ----------------------------------------------------------------------------
    [Header("Networked Variables")]
    public Vector3 currentPos;
    public Vector3 prevPos;   

    [Header("Client Prediction")]
    private float timer;
    private int currentTick;
    private float minTimeBetweenTicks;
    private const float SERVER_TICK_RATE = 60f;

    // ----------------------------------------------------------------------------
    // MONOBEHAVIOR
    // ----------------------------------------------------------------------------

    protected override void Awake()
    {
        base.Awake();
        
        // Initialize Animation Handler
        animationHandler = GetComponent<AnimationHandler>();
        animationHandler.Initialize(this, animationHandler.animator, true);

        // Initialize InteractionHandler
        interactionHandler = GetComponent<InteractionHandler>();
        interactionHandler.Initialize(this);

        // Initialize ManeuverHandler
        maneuverHandler = GetComponent<ManeuverHandler>();
        maneuverHandler.Initialize(this, animationHandler.animator, true);

        // Initialize InputHandler
        inputHandler = GetComponent<InputHandler>();
        inputHandler.Initialize(this, true);
        inputHandler.JumpCallback += Jump;
    }

    protected override void Start()
    {
        // Assign static LocalController variable
        if (Client.IsLocalPlayer(identity))
        {
            Local = this;
            GameManager.GetInstance().pcRef = this;
        }


        // Calculate MS ticks for Server
        minTimeBetweenTicks = 1f / SERVER_TICK_RATE;

        // Only adjust certain settings for the local player
        if (Client.IsLocalPlayer(identity))
        {                       
            // Toggle Cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;            
        }

        else
        {
            // Hide the camera from remote players
            cam.enabled = false;
        }
    }

    protected override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= minTimeBetweenTicks)
        {
            timer -= minTimeBetweenTicks;
            HandleUpdate();
            currentTick++;
        }
    }

    private void HandleUpdate()
    {                 
        // Prevent other clients from calling this code.
        if (!Client.IsLocalPlayer(identity)) return;

        // Update Manager Code
        interactionHandler.Tick();
        maneuverHandler.Tick();

        // Process Controller Animations
        HandleAnimations();        

        HandleCamera();
        if (!maneuverHandler.b_IsSplineControlled && !maneuverHandler.b_IsLedgeHandling)
        {            
            HandleMotion();            
        }

        // Broadcast the networked position and rotation to the server
        ClientSend.PlayerMovement(transform.position, mesh.transform.rotation);
    }    

    public void SetPosition(Vector3 newPos)
    {
        prevPos = transform.position;
        currentPos = newPos;

        if (!Client.IsLocalPlayer(identity))
        {
            transform.position = newPos;
        }
    }
}
