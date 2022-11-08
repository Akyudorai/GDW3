using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wants to know when another object does something interesting 
public abstract class Observer
{
    public abstract void OnNotify(int state);
}

public class LampObserver : Observer
{
    Light lampLight;
    LampEvents timeEvent;

    public LampObserver(Light lampLight, LampEvents timeEvent)
    {
        this.lampLight = lampLight;
        this.timeEvent = timeEvent;

    }

    public override void OnNotify(int state)
    {
        switch (state) 
        {
            case 0: // NIGHT
                ChangeLamp(LampEvents.Night.GetColor(), LampEvents.Night.GetIntensity());
                break;
            default:
            case 1: // DAY                
                ChangeLamp(LampEvents.Day.GetColor(), LampEvents.Day.GetIntensity());
                break;
        }
    }

    private void ChangeLamp(Color mat, float bright)
    {
        lampLight.color = mat;
        lampLight.intensity = bright;
    }
}
