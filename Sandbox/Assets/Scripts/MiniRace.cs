using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRace : MonoBehaviour
{
    public List<GameObject> _checkpoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetMiniRace()
    {
        foreach(GameObject obj in _checkpoints)
        {
            obj.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ResetMiniRace();
    }
}
