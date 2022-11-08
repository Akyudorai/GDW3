using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public Slider slider;
    public float duration = 5f;
    float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while(currentTime <= duration)
        {
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, currentTime / duration);
            currentTime += Time.deltaTime;
            Debug.Log(slider.value);
        }
    }
}
