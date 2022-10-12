using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SplineType 
{
    Rail,
    Wall,
    Zipline
}

public class SplinePath : MonoBehaviour
{
    [Header("Setup")]
    public SplineType splineType;
    public List<GameObject> points;
    [SerializeField] List<SplineNode> nodes = new List<SplineNode>();

    [Header("Values")]
    public int nodeIndex = 0;
    public PlayerController2 pcRef;

    [Header("Debugging")]
    public bool DebugSplinePath = false;

    private void Start() 
    {   
        // We only want to initialize for manually created splines, not generated spline paths (like wall running)
        if (splineType == SplineType.Rail || splineType == SplineType.Zipline) {
            Initialize();
        }
    }

    // Generates the interactables and collision components of the spline path
    public void Initialize() 
    {
        // Generate a spline node for each point based on position of points specified in the inspector
        GenerateNodes();

        // Generate collider, interaction data, and type-specific interactable scripts
        for (int i = 0; i < nodes.Count - 1; i++) 
        {
            GameObject pathCollider = new GameObject();
            pathCollider.name = "Path Collider";
            pathCollider.tag = "Interactable";            
            pathCollider.transform.position = nodes[i].position - (nodes[i].position - nodes[i].next.position)/2;
            pathCollider.transform.LookAt(nodes[i].next.position);

            BoxCollider col = pathCollider.AddComponent<BoxCollider>();
            col.size = new Vector3(0.1f, 0.1f, (nodes[i].position - nodes[i].next.position).magnitude);
            col.isTrigger = true;
            pathCollider.transform.SetParent(this.transform);

            switch (splineType) 
            {
                default:
                case SplineType.Rail:
                    RailInteractable rl_interactable = pathCollider.AddComponent<RailInteractable>();
                    rl_interactable.Initialize(nodes[i]);
                    break;
                case SplineType.Wall:

                    break;
                case SplineType.Zipline:
                    ZiplineInteractable zl_interactable = pathCollider.AddComponent<ZiplineInteractable>();
                    zl_interactable.Initialize(nodes[i]);
                    break;
            }                    
        }
    }

    private void Update() 
    {
        if (splineType == SplineType.Wall && pcRef == null) {
            Destroy(gameObject);
        }
    }

    // Return current node for use by the Player Controller
    public SplineNode GetCurrentNode()
    {
        return nodes[nodeIndex];
    }

    // Use recursion to generate spline nodes using GameObjects placed in inspector
    public SplineNode GenerateNodes(int index = 0) 
    {
        SplineNode result = new SplineNode();
        nodes.Add(result);
        result.index = index;
        result.position = points[index].transform.position;       
        result.previous = ((index > 0) ? nodes[index-1] : null);
        result.next = ((index < points.Count - 1) ? GenerateNodes(index+1) : null);                
        result.path = this;
        return result;
    }

    public void OnDrawGizmos() 
    {
        // As long as there is at least 2 objects acting as nodes, draw a line between them to visualize the path
        if (DebugSplinePath && points.Count >= 2) 
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(points[i].transform.position, points[i+1].transform.position);
            }
        }
    }


}
