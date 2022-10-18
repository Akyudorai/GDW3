using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScore : MonoBehaviour
{
    public static ChangeScore instance;
    public static int score = 0;
    // Start is called before the first frame update
    public static void AddPoint()
    {
        score++;
        UI_Manager.GetInstance().SetScoreDisplay(score);
    }
    public static void SubtractPoint()
    {
        score--;
        UI_Manager.GetInstance().SetScoreDisplay(score);
    }

    
}
