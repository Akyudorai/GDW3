using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsCharacterScript : MonoBehaviour
{
    public AnimationClip clipToPlay;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play(clipToPlay.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
