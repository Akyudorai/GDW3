using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    // ============ SINGLETON ============
    private static UI_Manager instance;
    
    public static UI_Manager GetInstance() 
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
        DontDestroyOnLoad(this.gameObject);
    }

    // ============ SINGLETON ============

    // References
    public TMP_Text MoneyDisplay;
    public TMP_Text InteractionDisplay;
    public GameObject notificationIcon;

    [Header("Screen Panel")]
    public GameObject ScreenPanel;
    public TMP_Text RaceTimer;

    [Header("Phone Panel")]
    public GameObject PhonePanel;
    public GameObject HomepagePanel, MapPanel, FastTravelPanel, MessengerPanel, MinigamePanel, MultiplayerPanel, SettingsPanel, QuitPanel, TipPanel;

    [Header("Quest Panel")]
    public GameObject questListPanel; //panel that shows all available/completed quest
    public GameObject questInfoPanel; //panel that shows details of selected quest
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questStatus;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questObjective;
    public TextMeshProUGUI questHint;
    public GameObject questItem1;
    public GameObject questItem2;
    public GameObject questItem3;
    public GameObject questLogItem;
    public GameObject contentPanel;
    public GameObject[] questItemIcons;

    [Header("Settings Panel")]
    public Slider bgmSlider;
    public Slider soundEffectSlider;


    [Header("Other Components")]
    public TMP_Text SpeedTracker;


    private void Start() 
    {
        InputManager.GetInput().Player.Escape.performed += cntxt => TogglePhonePanel(!PhonePanel.activeInHierarchy);
    
        EventManager.OnRaceBegin += EnableRaceTimer;
        EventManager.OnRaceEnd += DisableRaceTimer;

        questItemIcons = new GameObject[3];
        questItemIcons[0] = questItem1;
        questItemIcons[1] = questItem2;
        questItemIcons[2] = questItem3;
    }

    // ============ SCREEN PANEL FUNCTIONS =====================

    public void EnableRaceTimer(int id) 
    {
        Debug.Log("Race Timer Enabled");
        RaceTimer.enabled = true;
    }

    public void DisableRaceTimer(bool state) 
    {
        RaceTimer.enabled = false;
    }

    public void UpdateRaceTimer(float time) 
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.RoundToInt(time % 60);
        RaceTimer.text = minutes + ":" + ((seconds<10) ? "0" + seconds : seconds);
    }
    

    // ============ PHONE PANEL FUNCTIONS =====================

    public void TogglePhonePanel(bool state) 
    {
        PhonePanel.SetActive(state);
        GameManager.GetInstance().Pause(state);
    }
    
    public void ToggleMapPanel(bool state) 
    {
        MapPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        Debug.Log("Map Panel Clicked");
    }

    public void ToggleFastTravelPanel(bool state) 
    {
        FastTravelPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }
    
    public void ToggleMessengerPanel(bool state) 
    {
        MessengerPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    public void ToggleMinigamePanel(bool state) 
    {
        MinigamePanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    public void ToggleMultiplayerPanel(bool state) 
    {
        MultiplayerPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    public void ToggleSettingsPanel(bool state) 
    {
        SettingsPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    public void ToggleTipPanel(bool state)
    {
        TipPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    // ============ QUEST PANEL FUNCTIONS =====================

    public void ToggleQuestInfoPanel(bool state) 
    {
        questInfoPanel.SetActive(state);
    }

    public void ToggleQuestListPanel(bool state)
    {
        questListPanel.SetActive(state);
    }

    public void UpdateQuestName(string name) 
    {
        questTitle.text = name;
    }

    public void UpdateQuestDescription(string description)
    {
        questDescription.text = description;
    }

    public void UpdateQuestObjective(string objective) 
    {
        questObjective.text = objective;
    }

    // ============ OTHER COMPONENTS =====================

    public void UpdateSpeedTracker(float speed) 
    {
        SpeedTracker.text = speed.ToString("F2");
    }

    // ============ QUIT PANEL =====================

    public void ToggleQuitPanel(bool state) 
    {
        QuitPanel.SetActive(state);
    }    
    
    public void Quit() 
    {
        // Return to Main Menu Instead?

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }

    // =================================

    public void ResetPosition() 
    {
        GameManager.GetInstance().RespawnPlayer();
    }

    // Other Functions
    public void ToggleInteraction(bool state) 
    {
        InteractionDisplay.gameObject.SetActive(state);
    }

    public void SetInteractionDisplay(string text) 
    {
        InteractionDisplay.text = text;
    }

    public void SetMoneyDisplay(int value) 
    {
        MoneyDisplay.text = "Money: " + value;
    }

    public void FadeInNotification()
    {
        notificationIcon.GetComponent<Animation>().Play("notificationFadeIn");
    }

    public void FadeOutNotification()
    {
        notificationIcon.SetActive(true);
        notificationIcon.GetComponent<Animation>().Play("notificationFadeOut");
    }

    // ============ SETTINGS PANEL FUNCTIONS =====================

    public void AdjustBGMVolume()
    {
        SoundManager.GetInstance().backgroundMusic.setParameterByName("BGMVolume", bgmSlider.value);
        Debug.Log(bgmSlider.value);
    }

    public void AdjustSoundEffectVolume()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SoundEffect", soundEffectSlider.value);
    }
}
