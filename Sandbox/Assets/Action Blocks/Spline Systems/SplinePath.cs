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
    public bool IsLooping = false;

    [Header("Wall Spline Specific")]
    public bool isRight = false;
    

    [Header("Debugging")]
    public bool DebugSplinePath = false;
    private LineRenderer pathLine;

    public bool isGenerated = true;

    private void Awake() 
    {   
        if (isGenerated) return;

        // We only want to initialize for manually created splines, not generated spline paths (like wall running)
        if (splineType == SplineType.Rail || splineType == SplineType.Zipline) {
            Initialize();
        }

        if (GetComponent<LineRenderer>() != null) {
            pathLine = GetComponent<LineRenderer>();
            pathLine.positionCount = points.Count;
            for (int i = 0 ; i < points.Count; i++) {
                pathLine.SetPosition(i, points[i].transform.position);
            }
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

        if (index == points.Count - 1 && IsLooping) 
        {            
            nodes[0].previous = result;
            result.next = nodes[0];
        }

        return result;
    }

    // Unbind our player to the spline and allow it to return to normal controls
	public void Detatch() 
	{   
		// If the spline type is generated 
        if (splineType == SplineType.Wall) 
		{
			// Destroy the generated spline after being done with it.
			Destroy(this.gameObject);
		}								
	}

    // Return current node for use by the Player Controller
    public SplineNode GetNode(int index)
    {        
        return nodes[index];
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

            if (IsLooping == true) 
            {
                Gizmos.DrawLine(points[points.Count-1].transform.position, points[0].transform.position);   
            }
        }
    }


}
