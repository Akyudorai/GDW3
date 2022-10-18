using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Player")
        {
            UI_Manager.GetInstance().enableTimer = true;
            Destroy(gameObject);
        }
    }
}
