using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayMenuScript : MonoBehaviour
{
    public GameObject title;
    public GameObject image;
    public AnimationClip activeClip;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play(activeClip.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointEnter()
    {
        Debug.Log("Enter");
    }

    public void ActiveSelection()
    {
        
    }


    
}

    
