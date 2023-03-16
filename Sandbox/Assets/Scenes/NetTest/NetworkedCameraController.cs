using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCameraController : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    // VARIABLES & COMPONENTS
    // ----------------------------------------------------------------------------

    public NetworkIdentity player;
    private NetworkedPlayerController netPC;

    public Transform t_camera;
    private Quaternion camRotation;
    private RaycastHit hit;
    private Vector3 camera_offset;

    public float cameraSmoothingFactor = 1;
    public float lookUpMax = 60;
    public float lookUpMin = -60;
    public Vector2 v_Rotation;

    // ----------------------------------------------------------------------------
    // MONOBEHAVIOR
    // ----------------------------------------------------------------------------

    private void Start()
    {
        netPC = player.GetComponent<NetworkedPlayerController>();

        camRotation = transform.localRotation;
        camera_offset = t_camera.localPosition;

        
    }

    private void Update()
    {
        Look();        
    }

    // ----------------------------------------------------------------------------
    // CAMERA CONTROLLER
    // ----------------------------------------------------------------------------

    private void Look()
    {
        //camRotation.x += v_Rotation.y * cameraSmoothingFactor * (-1);
        //camRotation.y += v_Rotation.x * cameraSmoothingFactor;

        //camRotation.x = Mathf.Clamp(camRotation.x, lookUpMin, lookUpMax);

        //transform.localRotation = Quaternion.Euler(camRotation.x, camRotation.y, camRotation.z);

        //if (Physics.Linecast(transform.position, transform.position + transform.localRotation * camera_offset, out hit))
        //{
        //    t_camera.localPosition = new Vector3(0, 0, -Vector3.Distance(transform.position, hit.point));
        //}

        //else
        //{
        //    t_camera.localPosition = camera_offset;
        //}
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.localRotation * camera_offset);
    }
}
