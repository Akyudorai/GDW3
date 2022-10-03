using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public GameObject respawnPoint;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            Debug.Log("Player OOB!");

            Rigidbody player_rigid = other.gameObject.GetComponent<Rigidbody>();
            other.gameObject.transform.position = respawnPoint.transform.position;
            player_rigid.velocity = Vector3.zero;
        }    
    }
}
