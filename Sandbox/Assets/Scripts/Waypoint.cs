using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [HideInInspector] public WaypointSystem system;

    public GameObject PointerObj = null;

    private void Awake() 
    {
        PointerObj.SetActive(false);
    }

    public void ActivatePointer(GameObject target) 
    {
        PointerObj.SetActive(true);
        PointerObj.transform.LookAt(target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Play Vending machine interact sound
        FMOD.Studio.EventInstance waypointSFX = SoundManager.CreateSoundInstance(SoundFile.Waypoint);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(waypointSFX, this.gameObject.transform, GameManager.GetInstance().pcRef.rigid);
        waypointSFX.start();
        waypointSFX.release();

        if (other.tag == "Player") 
        {
            system.NextWaypoint();
        }
    }
}
