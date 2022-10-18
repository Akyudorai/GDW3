using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICommand
{
    void Execute();
    void IncreaseNum();
    void DecreaseNum();
}