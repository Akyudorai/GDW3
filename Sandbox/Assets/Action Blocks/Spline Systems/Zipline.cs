using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{
    [Header("Zipline Variables")]    
    public float ziplineSpeed = 1.0f;
    public GameObject pointA, pointB;

    [Header("Debugging")]
    public bool DebugZiplinePath = false;

    public void Start() 
    {
        /*
        // Generate a collider connecting the two points
        GameObject ziplineObject = new GameObject();
        ziplineObject.name = "Zipline Collider";
        ziplineObject.tag = "Interactable";
        ziplineObject.transform.position = pointA.transform.position - (pointA.transform.position - pointB.transform.position)/2;
        ziplineObject.transform.LookAt(pointB.transform.position);
        BoxCollider col = ziplineObject.AddComponent<BoxCollider>();
        col.size = new Vector3(0.1f, 0.1f, (pointA.transform.position - pointB.transform.position).magnitude);
        col.isTrigger = true;
        ziplineObject.transform.SetParent(this.transform);
        ZiplineInteractable interactable = ziplineObject.AddComponent<ZiplineInteractable>();
        interactable.Initialize(pointA, pointB);
        */
    }

    public void OnDrawGizmos()
    {
        if (DebugZiplinePath) 
        {
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }
}
