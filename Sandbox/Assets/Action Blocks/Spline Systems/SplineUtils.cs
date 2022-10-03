using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineUtils : MonoBehaviour
{
    public static SplinePath GenerateWallRunPath(Vector3 position, Vector3 direction, float speed) 
    {
        // Generate the new SplinePath object
        GameObject pathCollider = new GameObject();
        pathCollider.name = "Wall Run Path";
        SplinePath path = pathCollider.AddComponent<SplinePath>();
        path.splineType = SplineType.Wall;
        path.DebugSplinePath = true;

        // Create 5 points and generate a node for each
        GameObject p1 = new GameObject();
        p1.transform.position = position;
        GameObject p2 = new GameObject();
        p2.transform.position = position + direction * speed + Vector3.up;
        GameObject p3 = new GameObject();
        p3.transform.position = position + direction * speed * 2 + Vector3.up * 1.5f;
        GameObject p4 = new GameObject();
        p4.transform.position = position + direction * speed * 3 + Vector3.up;
        GameObject p5 = new GameObject();
        p5.transform.position = position + direction * speed * 4;
        
        // Populate the points for SplinePath
        path.points = new List<GameObject>();
        path.points.Add(p1);
        path.points.Add(p2);
        path.points.Add(p3);
        path.points.Add(p4);
        path.points.Add(p5);

        // Initialize the SplinePath
        path.Initialize();
        
        // SplineNode n1 = new SplineNode();
        // n1.index = 0; n1.position = p1; n1.path = path;
        // n1.previous = null;
        // SplineNode n2 = new SplineNode();
        // n2.index = 1; n2.position = p2; n2.path = path;
        // n1.next = n2; n2.previous = n1;
        // SplineNode n3 = new SplineNode();
        // n3.index = 2; n3.position = p3; n3.path = path;
        // n2.next = n3; n3.previous = n2;
        // SplineNode n4 = new SplineNode();
        // n4.index = 3; n4.position = p4; n4.path = path;
        // n3.next = n4; n4.previous = n3;
        // SplineNode n5 = new SplineNode();
        // n5.index = 4; n5.position = p5; n5.path = path;
        // n4.next = n5; n5.previous = n4; n5.next = null;

        // // Populate the path with the spline points
        // path.nodes = new List<SplineNode>();
        // path.nodes.Add(n1); 
        // path.nodes.Add(n2);
        // path.nodes.Add(n3);
        // path.nodes.Add(n4);
        // path.nodes.Add(n5);

        return path;
    } 
}
