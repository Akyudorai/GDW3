using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineUtils : MonoBehaviour
{
    public static SplinePath GenerateWallRunPath(Vector3 position, Vector3 direction, float speed, bool isForward) 
    {
        // Generate the new SplinePath object
        GameObject pathCollider = new GameObject();
        pathCollider.name = "Wall Run Path";
        SplinePath path = pathCollider.AddComponent<SplinePath>();
        path.splineType = SplineType.Wall;
        path.DebugSplinePath = true;
   
        // Generate 5 points
        GameObject p1 = new GameObject();
        p1.transform.SetParent(pathCollider.transform);
        GameObject p2 = new GameObject();
        p2.transform.SetParent(pathCollider.transform);
        GameObject p3 = new GameObject();
        p3.transform.SetParent(pathCollider.transform);
        GameObject p4 = new GameObject();
        p4.transform.SetParent(pathCollider.transform);
        GameObject p5 = new GameObject();
        p5.transform.SetParent(pathCollider.transform);

        // Generate a directional inverter value
        float inverter = ((isForward) ? 1 : -1);

        // Set the positions of each point relative to direction, travel      
        p1.transform.position = position;
        p2.transform.position = position + direction * speed * inverter + Vector3.up;
        p3.transform.position = position + direction * speed * 2 * inverter + Vector3.up * 1.5f;
        p4.transform.position = position + direction * speed * 3 * inverter + Vector3.up;
        p5.transform.position = position + direction * speed * 4 * inverter; 

        // Populate the points for SplinePath
        path.points = new List<GameObject>();
        path.points.Add(p1);
        path.points.Add(p2);
        path.points.Add(p3);
        path.points.Add(p4);
        path.points.Add(p5);

        // Initialize the SplinePath
        path.Initialize();

        return path;
    } 
}
