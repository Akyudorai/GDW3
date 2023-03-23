using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentRootMotion : MonoBehaviour
{
    public bool useRootMotion = false;
    Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    /*
    public void Update()
    {
        animator.applyRootMotion = useRootMotion;
    }
    */
    public void OnAnimatorMove()
    {
        Debug.LogWarning("OnAnimatorMove Called!");

        /*
        if (animator && useRootMotion) 
        {
            Debug.LogWarning("OnAnimatorMove: Updating Position of parent");
            //transform.parent.rotation = animator.rootRotation;
            transform.parent.position += animator.deltaPosition;            
        }  
        */
    }
}
