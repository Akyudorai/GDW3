using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSystem : MonoBehaviour
{
    /*
    [Header("Rail Nodes")]     
    public List<GameObject> points = new List<GameObject>();
    public List<SplineNode> nodes = new List<SplineNode>();

    [Header("Debugging")]
    public bool DebugRailPath = false;

    public void Start() 
    {
        // Generate a spline node for each point       
        GenerateNodes(0);

        for (int i = 0; i < nodes.Count - 1; i++) 
        {
            GameObject railCollider = new GameObject();
            railCollider.name = "Rail Collider";
            railCollider.tag = "Interactable";
            railCollider.transform.position = nodes[i].position - (nodes[i].position - nodes[i].next.position)/2;
            railCollider.transform.LookAt(nodes[i].next.position);

            BoxCollider col = railCollider.AddComponent<BoxCollider>();
            col.size = new Vector3(0.1f, 0.1f, (nodes[i].position - nodes[i].next.position).magnitude);
            col.isTrigger = true;
            railCollider.transform.SetParent(this.transform);

            RailInteractable interactable = railCollider.AddComponent<RailInteractable>();         
        }
    }

    public SplineNode GenerateNodes(int index) 
    {
        SplineNode result = new SplineNode();
        nodes.Add(result);
        result.index = index;
        result.position = points[index].transform.position;       
        result.previous = ((index > 0) ? nodes[index-1] : null);
        result.next = ((index < points.Count - 1) ? GenerateNodes(index+1) : null);                
        return result;
    }

    public void OnDrawGizmos()
    {        
        if (DebugRailPath && points.Count >= 2)  
        {
            for (int i = 0; i < points.Count - 1; i++) 
            {
                Gizmos.DrawLine(points[i].transform.position, points[i+1].transform.position);
            }
            
        }
    }
    */
}
