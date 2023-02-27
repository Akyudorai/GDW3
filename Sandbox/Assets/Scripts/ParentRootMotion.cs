using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentRootMotion : MonoBehaviour
{
    public bool useRootMotion = false;

    public void OnAnimatorMove()
    {
        Debug.LogWarning("OnAnimatorMove Called!");
        Animator animator = GetComponent<Animator>();

        if (animator && useRootMotion) 
        {
            Debug.LogWarning("OnAnimatorMove: Updating Position of parent");
            //transform.parent.rotation = animator.rootRotation;
            transform.parent.position += animator.deltaPosition;            
        }        
    }
}
