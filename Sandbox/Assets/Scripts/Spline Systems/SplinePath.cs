using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SplineType 
{
    Rail,
    Wall,
    Zipline,
    Ledge
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

    public bool isGenerated = true; // For Wall Splines only (and any other that get created during run time)
    public bool generatePathMesh = false;

    private void Awake() 
    {   
        if (isGenerated) return;

        // We only want to initialize for manually created splines, not generated spline paths (like wall running)
        if (splineType == SplineType.Rail || splineType == SplineType.Zipline || splineType == SplineType.Ledge) {
            Initialize();
        }

        if (GetComponent<LineRenderer>() != null) {
            pathLine = GetComponent<LineRenderer>();
            pathLine.positionCount = points.Count;
            for (int i = 0 ; i < points.Count; i++) {
                pathLine.SetPosition(i, points[i].transform.position);
            }
            pathLine.enabled = true;
        }
    }

    // Generates the interactables and collision components of the spline path
    public void Initialize() 
    {
        // Generate a spline node for each point based on position of points specified in the inspector
        GenerateNodes();

        // Generate collider, interaction data, and type-specific interactable scripts
        for (int i = 0; i < ((IsLooping) ? nodes.Count : nodes.Count - 1); i++) 
        {
            SplineNode next = ((IsLooping && i == nodes.Count-1) ? nodes[0] : nodes[i].next);

            GameObject pathCollider = new GameObject();
            pathCollider.name = "Path Collider";
            pathCollider.tag = "Interactable";  
            pathCollider.layer = LayerMask.NameToLayer("Interactable");          
            pathCollider.transform.position = nodes[i].position - (nodes[i].position - next.position)/2;
            pathCollider.transform.LookAt(nodes[i].next.position);

            BoxCollider col = pathCollider.AddComponent<BoxCollider>();
            col.size = new Vector3(0.1f, 0.1f, (nodes[i].position - next.position).magnitude);
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
                case SplineType.Ledge:
                    LedgeInteractable ldg_interactable = pathCollider.AddComponent<LedgeInteractable>();
                    ldg_interactable.Initialize(nodes[i]);
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
            Color c = Color.white;
            if (splineType == SplineType.Rail) c = Color.yellow;
            if (splineType == SplineType.Zipline) c = Color.blue;
            if (splineType == SplineType.Ledge) c = Color.green;

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
