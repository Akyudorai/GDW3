using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform start, finish;
    bool reverse = false;
    float time = 0.0f;
    bool reverseDelay = false;    

    [Range(0.0f, 1.0f)]
    public float speed;

    public void Update() 
    {     
        if (transform.position != ((reverse) ? start.position : finish.position)) 
        {            
            transform.position = Vector3.Lerp(start.position, finish.position, time);
            time += speed * ((reverse) ? -Time.deltaTime : Time.deltaTime);            
        }

        else 
        {
            if (!reverseDelay) {
                StartCoroutine(DelayReverse());
            }                        
        }
    }

    public float GetPlatformSpeed()
    {
        float timeStep = speed * ((reverse) ? -Time.deltaTime : Time.deltaTime);        
        float distance = Vector3.Distance(start.localPosition, finish.localPosition);
        if (reverseDelay) return 0;
        return Mathf.Abs(timeStep * distance);
    }

    public Vector3 GetPlatformMoveDirection()
    {
        Vector3 direction = start.localPosition - (start.localPosition - finish.localPosition);
        if (reverse) direction *= -1;
        if (reverseDelay) direction = Vector3.zero;
        return direction.normalized;
    }

    private IEnumerator DelayReverse() 
    {        
        reverseDelay = true;
        yield return new WaitForSeconds(1.0f);
        reverseDelay = false;
        reverse = !reverse;        
    }

}
