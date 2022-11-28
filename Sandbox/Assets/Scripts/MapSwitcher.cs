using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcher : MonoBehaviour
{
    public LayerMask MaskToSwitch;
    public Camera TargetCamera;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player") 
        {
            TargetCamera.cullingMask = MaskToSwitch | 1 << LayerMask.NameToLayer("MapTracker");
        }    
    }
}
