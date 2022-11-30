using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dep_AnimatedPlayerController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Rigidbody rigid;
    public CapsuleCollider col;
    public GameObject camera_pivot;

    [Header("Movement")]
    public float f_MoveSpeed;
    private Vector3 movement;
    

    [Header("Camera")]
    public float f_MouseSensitivity;
    public bool b_InvertMouse;

    [Header("States")]
    public bool b_Vaulting;
    public bool b_Sliding;
    public bool b_Jumping;
    public bool b_IsGrounded;
    private float f_AirTime = 0.0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update() 
    {
        Movement();
        Camera();
        Controls();

        // Ground Check
        int layerMask = 1 << 6;
        if (Physics.Raycast(col.transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.5f, layerMask))
        {
            b_IsGrounded = true;
            animator.SetBool("IsGrounded", true);
            animator.SetTrigger((f_AirTime > 1.0f) ? "Land-Hard" : "Land-Soft");
            f_AirTime = 0.0f;            
        }

        if (!b_IsGrounded) {
            f_AirTime += Time.deltaTime;
        }
    }

    public void Controls()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !b_Jumping) 
        {
            // Vault
            animator.SetTrigger("Jump");
            animator.SetBool("IsGrounded", false);
        }        
    }

    private IEnumerator Vault() 
    {
        b_Vaulting = true;
        yield return new WaitForSeconds(1.0f);
        b_Vaulting = false;
    }

    private IEnumerator Slide() 
    {
        b_Sliding = true;
        col.height = 0.5f;        
        yield return new WaitForSeconds(1.0f);
        col.height = 2.0f;        
        b_Sliding = false;
    }

    public void Movement()
    {         
        if (b_Vaulting || b_Sliding) return;

        movement.x = Input.GetAxis("Horizontal");        
        movement.z = Input.GetAxis("Vertical");        

        if (movement == Vector3.zero) return; 

        Vector3 forward = camera_pivot.transform.forward;
        Vector3 right = camera_pivot.transform.right;

        Vector3 forwardRelativeMovement = movement.z * forward;
        Vector3 rightRelativeMovement = movement.x * right;

        Vector3 cameraRelativeMovement = forwardRelativeMovement + rightRelativeMovement;        
        cameraRelativeMovement.y = 0;     

        //==========================

        transform.localEulerAngles = new Vector3(0, camera_pivot.transform.eulerAngles.y, 0);
        camera_pivot.transform.localEulerAngles = new Vector3(camera_pivot.transform.localEulerAngles.x, 0, 0);
        animator.SetFloat("Movement-X", movement.x);
        animator.SetFloat("Movement-Z", movement.z);

        //==========================                

        rigid.MovePosition(transform.position + cameraRelativeMovement * f_MoveSpeed * Time.fixedDeltaTime);        
    }

    public void Camera()
    {
        //transform.Rotate(0, Input.GetAxis("Mouse X") * f_MouseSensitivity, 0);        
        //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        float cameraX = Input.GetAxis("Mouse Y") * f_MouseSensitivity * ((b_InvertMouse) ? 1 : -1);
        float cameraY = Input.GetAxis("Mouse X") * f_MouseSensitivity;
        camera_pivot.transform.Rotate(cameraX, cameraY, 0);        
        camera_pivot.transform.localEulerAngles = new Vector3(ClampAngle(camera_pivot.transform.localEulerAngles.x, -60, 30), camera_pivot.transform.localEulerAngles.y, 0);        
    }

    private float ClampAngle(float angle, float from, float to) 
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360+from);
        return Mathf.Min(angle, to);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Vault" && !b_Vaulting)
        {
            // Vault
            animator.SetTrigger("Vault");
            StartCoroutine(Vault());
        }

        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Slide" && !b_Sliding)
        {
            // Slide
            animator.SetTrigger("Slide");
            StartCoroutine(Slide());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(col.transform.position, col.transform.position -transform.up * 0.5f);
    }
}
