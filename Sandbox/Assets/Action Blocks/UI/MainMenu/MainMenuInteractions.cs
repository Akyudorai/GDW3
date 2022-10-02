using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuInteractions : MonoBehaviour
{
    public GameObject RunningSprite;
    public AnimationClip buttonSlideAnim;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(buttonSlideAnim.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MouseEnter()
    {
        GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Italic;
        RunningSprite.SetActive(true);
    }
    
    public void MouseExit()
    {
        GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        RunningSprite.SetActive(false);
    }

    public void ButtonPressed()
    {
       GetComponent<Animator>().Play(buttonSlideAnim.name);
    }
}
