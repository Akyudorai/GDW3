using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using UnityEditor;


public enum SoundFile 
{   
    // Ambience
    CityAmbience,
    VendingMachineHum,
    
    // Music
    LofiMusic,

    // Interactions
    RailStart,
    RailLoop,
    RailRelease,
    ZiplineStart,
    ZiplineLoop,
    ZiplineRelease,
    WallRun,
    WallRunRelease,
    JumpPad,

    // Events
    RaceCountdown,
    RaceStart,
    RaceFinish,
    Waypoint,
    CollectiblePickup,
    Yay,
    QuestComplete,

    // Player
    Jump,
    Run,
    LandSoft,
    LandMed,
    LandHard,

    // UI
    AppHighlight,
    AppClick,
    PhoneNotification
}

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
    public FMOD.Studio.EventInstance cityAmbience;

    [Header("Ambient Sounds")]
    [SerializeField] FMODUnity.EventReference amb_city;
    [SerializeField] FMODUnity.EventReference amb_vendingMachine;

    [Header("Music Sounds")]
    [SerializeField] FMODUnity.EventReference bgm_lofi;

    [Header("Interaction Sounds")]
    [SerializeField] FMODUnity.EventReference int_railStart;
    [SerializeField] FMODUnity.EventReference int_railLoop;
    [SerializeField] FMODUnity.EventReference int_railRelease;
    [SerializeField] FMODUnity.EventReference int_zipStart;
    [SerializeField] FMODUnity.EventReference int_zipLoop;
    [SerializeField] FMODUnity.EventReference int_zipRelease;
    [SerializeField] FMODUnity.EventReference int_wallRun;
    [SerializeField] FMODUnity.EventReference int_wallRunRelease;
    [SerializeField] FMODUnity.EventReference int_jumpPad;

    [Header("Event Sounds")]
    [SerializeField] FMODUnity.EventReference eve_raceCountdown;
    [SerializeField] FMODUnity.EventReference eve_raceStart;
    [SerializeField] FMODUnity.EventReference eve_raceFinish;
    [SerializeField] FMODUnity.EventReference eve_waypoint;
    [SerializeField] FMODUnity.EventReference eve_collectiblePickup;
    [SerializeField] FMODUnity.EventReference eve_yay;
    [SerializeField] FMODUnity.EventReference eve_questComplete;

    [Header("Player Sounds")]
    [SerializeField] FMODUnity.EventReference pl_jump;
    [SerializeField] FMODUnity.EventReference pl_running;
    [SerializeField] FMODUnity.EventReference pl_landSoft;
    [SerializeField] FMODUnity.EventReference pl_landMed;
    [SerializeField] FMODUnity.EventReference pl_landHard;

    [Header("UI Sounds")]
    [SerializeField] FMODUnity.EventReference ui_appHiglight;
    [SerializeField] FMODUnity.EventReference ui_appClick;
    [SerializeField] FMODUnity.EventReference ui_phoneNotification;

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
        backgroundMusic = CreateSoundInstance(SoundFile.LofiMusic);
        //backgroundMusic.start();
        //backgroundMusic.setParameterByName("BGMVolume", 0);

        UpdateSoundSettings();

        cityAmbience = CreateSoundInstance(SoundFile.CityAmbience);
        cityAmbience.start();
    }

    private void OnDestroy() 
    {
        backgroundMusic.release();
        cityAmbience.release();
    }

    public static FMOD.Studio.EventInstance CreateSoundInstance(SoundFile file) 
    {
        switch (file) 
        {   
            // Ambience
            case SoundFile.CityAmbience: return FMODUnity.RuntimeManager.CreateInstance(instance.amb_city);
            case SoundFile.VendingMachineHum: return FMODUnity.RuntimeManager.CreateInstance(instance.amb_vendingMachine);
            
            // Music
            case SoundFile.LofiMusic: return FMODUnity.RuntimeManager.CreateInstance(instance.bgm_lofi);

            // Interactions
            case SoundFile.RailStart: return FMODUnity.RuntimeManager.CreateInstance(instance.int_railStart);
            case SoundFile.RailLoop: return FMODUnity.RuntimeManager.CreateInstance(instance.int_railLoop);
            case SoundFile.RailRelease: return FMODUnity.RuntimeManager.CreateInstance(instance.int_railRelease);
            case SoundFile.ZiplineStart: return FMODUnity.RuntimeManager.CreateInstance(instance.int_zipStart);
            case SoundFile.ZiplineLoop: return FMODUnity.RuntimeManager.CreateInstance(instance.int_zipLoop);
            case SoundFile.ZiplineRelease: return FMODUnity.RuntimeManager.CreateInstance(instance.int_zipRelease);
            case SoundFile.WallRun: return FMODUnity.RuntimeManager.CreateInstance(instance.int_wallRun);
            case SoundFile.WallRunRelease: return FMODUnity.RuntimeManager.CreateInstance(instance.int_wallRunRelease);
            case SoundFile.JumpPad: return FMODUnity.RuntimeManager.CreateInstance(instance.int_jumpPad);

            // Events
            case SoundFile.RaceCountdown: return FMODUnity.RuntimeManager.CreateInstance(instance.eve_raceCountdown);
            case SoundFile.RaceStart: return FMODUnity.RuntimeManager.CreateInstance(instance.eve_raceStart);
            case SoundFile.RaceFinish: return FMODUnity.RuntimeManager.CreateInstance(instance.eve_raceFinish);
            case SoundFile.Waypoint: return FMODUnity.RuntimeManager.CreateInstance(instance.eve_waypoint);
            case SoundFile.CollectiblePickup: return FMODUnity.RuntimeManager.CreateInstance(instance.eve_collectiblePickup);
            case SoundFile.Yay: return FMODUnity.RuntimeManager.CreateInstance(instance.eve_yay);
            case SoundFile.QuestComplete: return FMODUnity.RuntimeManager.CreateInstance(instance.eve_questComplete); 

            // Player
            case SoundFile.Jump: return FMODUnity.RuntimeManager.CreateInstance(instance.pl_jump);
            case SoundFile.Run: return FMODUnity.RuntimeManager.CreateInstance(instance.pl_running);
            case SoundFile.LandSoft: return FMODUnity.RuntimeManager.CreateInstance(instance.pl_landSoft);
            case SoundFile.LandMed: return FMODUnity.RuntimeManager.CreateInstance(instance.pl_landMed);
            case SoundFile.LandHard: return FMODUnity.RuntimeManager.CreateInstance(instance.pl_landHard);

            //UI
            case SoundFile.AppHighlight: return FMODUnity.RuntimeManager.CreateInstance(instance.ui_appHiglight);
            case SoundFile.AppClick: return FMODUnity.RuntimeManager.CreateInstance(instance.ui_appClick);
            case SoundFile.PhoneNotification: return FMODUnity.RuntimeManager.CreateInstance(instance.ui_phoneNotification);
        }

        Debug.LogError("No sound file [" + file.ToString() + "] found.  Did you set the reference?");
        return FMODUnity.RuntimeManager.CreateInstance(instance.eve_yay);
    }

    public void UpdateSoundSettings() 
    {        
        SettingsData Settings = SaveManager.Settings;

        float bgmVolume = ((Settings.Mute_Audio) ? 0f :  Mathf.Min(Settings.Master_Volume, Settings.BGM_Volume));
        backgroundMusic.setParameterByName("BGMVolume", bgmVolume);

        float sfxVolume = ((Settings.Mute_Audio) ? 0f : Mathf.Min(Settings.Master_Volume, Settings.SFX_Volume));
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SoundEffect", sfxVolume);
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
}
