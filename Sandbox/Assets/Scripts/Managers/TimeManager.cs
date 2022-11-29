using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class TimeManager : MonoBehaviour
{
    // ============= SINGLETON ================
    
    private static TimeManager instance;
    public static TimeManager GetInstance() 
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

    // ============= SINGLETON ================
    
    // DLL
    [SerializeField] private TimeCycle dayCycle;

    [Header("Time Components")]
    public Light sun;
    public Light moon;
    public Volume skyVolume;
    public PhysicallyBasedSky sky;

    [Header("Time Variables")]
    public float timeOfDay;
    public bool isNight;

    [Header("Observers")]
    public Subject observerSubject = new Subject();



    private void Start() 
    {
        dayCycle.InitializeDayCycle();
        skyVolume.profile.TryGet(out sky);
        timeOfDay = 86400.0f/2;
        Tick();
        

        EventManager.OnDayBegin += StartDay;
        EventManager.OnNightBegin += StartNight;
    }

    private void Tick() 
    {
        float alpha = timeOfDay / 86400.0f;
        float sunRotation = Mathf.Lerp(-90, 270, alpha);
        float moonRotation = sunRotation - 180;

        sun.transform.rotation = Quaternion.Euler(sunRotation, -150.0f, 0);
        moon.transform.rotation = Quaternion.Euler(moonRotation, -150.0f, 0);

        CheckDayNightTransition();
        
    }

    private void Update() 
    {
        //dayCycle.Tick();
        //timeOfDay = dayCycle.CurrentTime;
        

        Tick();
    }

    private void OnValidate() 
    {
        skyVolume.profile.TryGet(out sky);
        Tick();
    }

    private void CheckDayNightTransition() 
    {
        if (isNight) 
        {
            if (moon.transform.rotation.eulerAngles.x > 180)
            {
                EventManager.OnNightBegin?.Invoke();
            }
        }

        else 
        {
            if (sun.transform.rotation.eulerAngles.x > 180)
            {
                EventManager.OnDayBegin?.Invoke();
            }
        }
    }

    private void StartDay()
    {
        isNight = false;
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;

        // 1 = Day
        observerSubject.Notify(1);
    }

    private void StartNight() 
    {
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;

        // 0 = Night
        observerSubject.Notify(0);
    }

    public void SetTimeOfDay(float hour) 
    {
        if (hour < 0 || hour > 24) {
            Debug.Log("Time must be between 0 and 24.");
            return;
        }

        // 0 - 24
        float result = hour * dayCycle.GetCycleLength() / 24; 
        dayCycle.SetTime(result);
    }

    private void OnDestroy() 
    {
        dayCycle.Delete();
    }

}
