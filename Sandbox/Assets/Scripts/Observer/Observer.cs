using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wants to know when another object does something interesting 
public abstract class Observer
{
    public abstract void OnNotify();
}

public class Lamp : Observer
{
    Light lampLight;
    LampEvents timeEvent;

    public Lamp(Light lampLight, LampEvents timeEvent)
    {
        this.lampLight = lampLight;
        this.timeEvent = timeEvent;

    }

    public override void OnNotify()
    {
        LampChanges(timeEvent.LampColor(), timeEvent.LampIntensity());
    }

    void LampChanges(Color mat, float bright)
    {
        lampLight.GetComponent<Light>().color = mat;
        lampLight.GetComponent<Light>().intensity = bright;
    }
}
