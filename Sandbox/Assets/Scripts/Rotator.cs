using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float RotationRate = 1;
    public Vector3 RotationAxis = Vector3.up;

    public void Update() 
    {
        transform.Rotate(RotationAxis*RotationRate*Time.deltaTime);
    }
}
