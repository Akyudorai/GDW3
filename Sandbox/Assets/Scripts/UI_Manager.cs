using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    // ============ SINGLETON ============
    private static UI_Manager instance;
    public float timeDuration = 3f;
    public float timer;
    public bool enableTimer = false;
    public static UI_Manager GetInstance() 
    {
        return instance;
    }

    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    // ============ SINGLETON ============

    // References
    public TMP_Text ScoreDisplay;
    public TMP_Text InteractionDisplay;

    public void ToggleInteraction(bool state) 
    {
        InteractionDisplay.gameObject.SetActive(state);
    }

    public void SetInteractionDisplay(string text) 
    {
        InteractionDisplay.text = text;
    }

    public void SetScoreDisplay(int score) 
    {
        ScoreDisplay.text = "Score: " + score;
    }
    
}
