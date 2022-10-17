using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyText : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(10f, 10f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
