using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCard : MonoBehaviour
{
    GameObject matchManager;
    public GameObject icon;
    // Start is called before the first frame update
    void Start()
    {
        //matchManager = GameObject.Find("MatchManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipCard()
    {
        icon.SetActive(true);
        matchManager = GameObject.Find("MatchManager");
        matchManager.GetComponent<MatchManager>().AddCard(this.gameObject);
    }

    public void UnFlipCard()
    {
        icon.SetActive(false);
    }
}
