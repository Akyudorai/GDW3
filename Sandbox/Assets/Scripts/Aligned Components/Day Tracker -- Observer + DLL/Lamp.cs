using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public Light lampLight;
    public LampObserver observer;

    private void Start() 
    {
        int state = ((TimeManager.GetInstance().isNight) ? 0 : 1);

        observer = new LampObserver(lampLight, (state == 0) ? 
            new LampEvents.Night() : new LampEvents.Day());

        TimeManager.GetInstance().observerSubject.AddObserver(observer);
        TimeManager.GetInstance().observerSubject.Notify(state);
    }

}
