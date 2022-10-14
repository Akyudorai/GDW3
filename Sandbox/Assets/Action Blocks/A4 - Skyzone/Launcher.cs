using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float LaunchForce = 10.0f;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            PlayerController2 pc = collision.gameObject.GetComponent<PlayerController2>();
            pc.rigid.velocity = new Vector3(pc.rigid.velocity.x, 0, pc.rigid.velocity.z);
            
            Vector3 launchVector = transform.up * LaunchForce;
            pc.ApplyForce(launchVector, ForceMode.Impulse);
        }
    }
}
