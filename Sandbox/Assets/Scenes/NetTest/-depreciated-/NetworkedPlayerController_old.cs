using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class NetworkedPlayerController_old : MonoBehaviour
{    
    [Header("Identity")]
    public string GUID = "";    
    private bool isLocalPlayer { get { return (GUID == NetTest.localGuid);}}

    [Header("Components")]
    public Camera cam;
    public GameObject camera_pivot;
    public Rigidbody rigid;    
    public GameObject mesh;
    public Animator animator;

    [Header("States")]
    private static bool isInitialized = false;

    [Header("Camera")]
    public float f_MouseSensitivity = 1.0f;
    public bool b_InvertMouse = false;
    public Vector2 hCameraClamp = new Vector2(-60, 30);
    public Vector2 FovRange = new Vector2(60, 90);
    public Vector2 v_Rotation;
    
    [Header("Motion")]
    public Vector2 v_MotionInput = Vector2.zero;
    public float f_Speed = 3.0f;
    

    public void Initialize(string id) 
    {   
        GUID = id;        

        Debug.Log("Initialize: " + isLocalPlayer);
        if (isLocalPlayer) 
        {
            BindPlayerControls();
        }
        

        isInitialized = true;
    }

    private void Update() 
    {
        if (!isInitialized || !isLocalPlayer) return;

        Movement();
        //Debug.Log("Local Player: " + isLocalPlayer);
    }

    private void Movement() 
    {
        transform.position += new Vector3(v_MotionInput.x, 0, v_MotionInput.y) * f_Speed * Time.deltaTime;
    }

    private void OnDestroy() 
    {
        UnbindPlayerControls();
    }

    private void BindPlayerControls() 
    {
        InputManager.GetInput().Player.Move.performed += MoveWithContext;
        InputManager.GetInput().Player.Move.canceled += MoveCancelWithContext;
        InputManager.GetInput().Player.Look.performed += LookWithContext;
        InputManager.GetInput().Player.Look.canceled += LookCancelWithContext;
    }

    private void UnbindPlayerControls() 
    {   
        if (InputManager.GetInput() == null) return;
        InputManager.GetInput().Player.Move.performed -= MoveWithContext;
        InputManager.GetInput().Player.Move.canceled -= MoveCancelWithContext;
        InputManager.GetInput().Player.Look.performed -= LookWithContext;
        InputManager.GetInput().Player.Look.canceled -= LookCancelWithContext;
    }

    private void MoveWithContext(InputAction.CallbackContext context) 
    {
        v_MotionInput = context.ReadValue<Vector2>();
    }

    private void MoveCancelWithContext(InputAction.CallbackContext context) 
    {
        v_MotionInput = Vector2.zero;
    }

    private void LookWithContext(InputAction.CallbackContext context) 
    {
        v_Rotation = context.ReadValue<Vector2>();
    }

    private void LookCancelWithContext(InputAction.CallbackContext context) 
    {
        v_Rotation = Vector2.zero;
    }    
}
