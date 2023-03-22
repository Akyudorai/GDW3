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

    private IEnumerator DelayReverse() 
    {        
        reverseDelay = true;
        yield return new WaitForSeconds(1.0f);
        reverseDelay = false;
        reverse = !reverse;        
    }

}
