using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float LaunchForce = 15.0f;
    public bool CanLaunch = true;

    private IEnumerator LaunchpadDelay() 
    {
        CanLaunch = false;
        yield return new WaitForSeconds(0.1f);
        CanLaunch = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && CanLaunch) 
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.v_VerticalVelocity = Vector3.zero;
            
            Vector3 launchVector = transform.up * LaunchForce;            
            pc.ApplyForce(launchVector, ForceMode.Impulse);            
            //StartCoroutine(pc.OverrideMovement(2.0f));
            StartCoroutine(LaunchpadDelay());
            StartCoroutine(pc.JumpDelay());
        }
    }
}
