using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayCar : MonoBehaviour
{
    [Header("Splines")]
    public SplineController splineController;
    public SplinePath startingSpline;
    public int startingIndex = 0;
    public GameObject mesh;
    public float traversalSpeed = 1.0f;


    private void Awake() 
    {
        splineController.mesh = mesh;
    }

    private void Start() 
    {
        startingSpline.GetNode(startingIndex).Attach(splineController);
    }

    private void Update() 
    {
        splineController.SetTraversalSpeed(traversalSpeed);
    }
}
