using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public int time = 0;
    public List<Light> lights = new List<Light>();
    public List<Lamp> lamps = new List<Lamp>();
    Lamp lamp1;
    Lamp lamp2;

    public Light sceneLighting = new Light();

    Subject subject = new Subject();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        Debug.Log(lights.Count);

        for (int i = 0; i < lights.Count; i++)
        {
            lamps.Add(new Lamp(lights[i], new Morning()));
            subject.AddObserver(lamps[i]);
        }
        subject.Notify();

    }

    // Update is called once per frame
    void Update()
    {
        if (time == 0) //morning
        {
            for (int i = 0; i < lights.Count; i++)
            {
                subject.RemoveObserver(lamps[i]);
                lamps[i] = new Lamp(lights[i], new Morning());
                subject.AddObserver(lamps[i]);
            }

            sceneLighting.intensity = 8826.203f;

        }

        if (time == 1) //afternoon
        {
            for (int i = 0; i < lights.Count; i++)
            {
                subject.RemoveObserver(lamps[i]);
                lamps[i] = new Lamp(lights[i], new Afternoon());
                subject.AddObserver(lamps[i]);
            }

            sceneLighting.intensity = 83493.91f;
        }

        if (time == 2) //night
        {
            for (int i = 0; i < lights.Count; i++)
            {
                subject.RemoveObserver(lamps[i]);
                lamps[i] = new Lamp(lights[i], new Night());
                subject.AddObserver(lamps[i]);
            }

            sceneLighting.intensity = 0f;
        }
        subject.Notify();
    }
}
