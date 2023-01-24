using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using UnityEditor;

public class SoundManager : MonoBehaviour
{   
    // Audio Library
    public Dictionary<string, AudioClip> AudioLib;
    public string FileDirectory = "Audio";

    // Audio Players Components
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    // Pitch Adjustment Range
    public float LowPitchRange = 0.95f;
    public float HighPitchRange = 1.05f;

    //Fmod instances
    public FMOD.Studio.EventInstance backgroundMusic;

    // Singleton Instance
    private static SoundManager instance;
    public static SoundManager GetInstance() 
    {
        return instance;
    }

    // Initialization
    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } 
     
        instance = this;
        AudioLib = new Dictionary<string, AudioClip>();
        PopulateAudioLib();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        backgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/BGM");
        backgroundMusic.start();

        backgroundMusic.setParameterByName("BGMVolume", UI_Manager.GetInstance().bgmSlider.value);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SoundEffect", UI_Manager.GetInstance().soundEffectSlider.value);
    }

    private void PopulateAudioLib()
    {        
        AudioClip[] clips = Resources.LoadAll<AudioClip>(FileDirectory);
        foreach (AudioClip c in clips) 
        {
            AudioLib.Add(c.name, c);       
        }
    }

    // Play a single clip through sound effects source
    public void Play(string index, bool randomPitch = false) 
    {
        if (AudioLib[index] != null) 
        {
            float newPitch = ((randomPitch) ? Random.Range(LowPitchRange, HighPitchRange) : 1.0f);
            EffectsSource.pitch = newPitch; 
            EffectsSource.clip = AudioLib[index];            
            EffectsSource.Play();  
        } 
        
        else  {
            Debug.Log("SoundManager: Clip [" + index + "] not found in Audio Library");            
        }
              
    }

    // Play a single clip through the music source
    public void PlayMusic(string index) 
    {
        if (AudioLib[index] != null) 
        {
            MusicSource.clip = AudioLib[index];
            MusicSource.Play();  
        } 
        
        else  {
            Debug.Log("SoundManager: Clip [" + index + "] not found in Audio Library");            
        }
    }

    public void AdjustBGMVolume(Slider _slider)
    {
        backgroundMusic.setParameterByName("BGMVolume", _slider.value);
        Debug.Log(_slider.value);
    }

    public void AdjustBGMVolume(float _value)
    {
        backgroundMusic.setParameterByName("BGMVolume", _value);
        Debug.Log(_value);
    }

    public void WallRunDetachSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/WallRunDetach");
    }
}
