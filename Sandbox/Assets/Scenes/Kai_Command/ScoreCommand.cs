using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCommand : ICommand
{
    public void Execute()
    {

    }
    public void IncreaseNum()
    {
        ChangeScore.AddPoint();
    }

    public void DecreaseNum()
    {
        ChangeScore.SubtractPoint();
    }
}
