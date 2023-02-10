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
    public int collectiblesCollected = 0;

    [Header("Screen Panel")]
    public GameObject ScreenPanel;
    public TMP_Text RaceTimer;

    [Header("Phone Panel")]
    public GameObject PhonePanel;
    public GameObject HomepagePanel, MapPanel, FastTravelPanel, MessengerPanel, MinigamePanel, MultiplayerPanel, SettingsPanel, QuitPanel, TipPanel;

    [Header("Home Panel")]
    public TMP_Text SearchBar;

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
    public GameObject ActivateQuestButton;

    [Header("Settings Panel")]
    public Slider bgmSlider;
    public Slider soundEffectSlider;

    [Header("Collectible Panel")]
    public Image Collectible1;
    public TMP_Text Collectible1Text;


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
        if(state == true)
        {
            PhonePanel.GetComponent<Animator>().SetBool("PhoneOpen", true);
        }
        else
        {
            PhonePanel.GetComponent<Animator>().SetBool("PhoneOpen", false);
        }
        GameManager.GetInstance().Pause(state);
    }
    
    public void ToggleMapPanel(bool state) 
    {
        MapPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
        Debug.Log("Map Panel Clicked");
    }

    public void ToggleFastTravelPanel(bool state) 
    {
        FastTravelPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
    }

    public void ToggleMessengerPanel(bool state) 
    {
        MessengerPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
    }

    public void ToggleMinigamePanel(bool state) 
    {
        MinigamePanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
    }

    public void ToggleMultiplayerPanel(bool state) 
    {
        MultiplayerPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
    }

    public void ToggleSettingsPanel(bool state) 
    {
        SettingsPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
    }

    public void ToggleTipPanel(bool state)
    {
        TipPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
    }

    public void ToggleHomePanel(bool state)
    {
        MapPanel.SetActive(!state);
        FastTravelPanel.SetActive(!state);
        MessengerPanel.SetActive(!state);
        MinigamePanel.SetActive(!state);
        MultiplayerPanel.SetActive(!state);
        SettingsPanel.SetActive(!state);
        TipPanel.SetActive(!state);
        QuitPanel.SetActive(!state);
        HomepagePanel.SetActive(state);
        SearchBar.gameObject.SetActive(state);
    }

    public void UpdateSearchBar(string _text)
    {
        SearchBar.text = _text;
    }

    // ============ QUEST PANEL FUNCTIONS =====================

    public void ToggleQuestInfoPanel(bool state) 
    {
        questInfoPanel.SetActive(state);
        if(state == false) //if returning to quest list panel, then the selected quest needs to be empty.
        {
            QuestManager.GetInstance().ClearSelectedQuest();
        }
    }

    public void ToggleQuestListPanel(bool state)
    {
        questListPanel.SetActive(state);
    }

    public void ToggleActivationButton(bool state)
    {
        ActivateQuestButton.SetActive(state);
    }

    public void UpdateQuestName(string name) 
    {
        questTitle.text = name;
    }

    public void UpdateQuestStatus(string status)
    {
        questStatus.text = status;
    }

    public void UpdateQuestDescription(string description)
    {
        questDescription.text = description;
    }

    public void UpdateQuestObjective(string objective) 
    {
        questObjective.text = objective;
    }

    public void UpdateQuestHint(string hint)
    {
        questHint.text = hint;
    }

    public void ActivateToggle()
    {
        QuestManager.GetInstance().ActivateQuest(QuestManager.GetInstance().questList[QuestManager.GetInstance().selectedQuest.questId]); //get the quest id from the quest data display object
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

    // == Test == //
    public void UpdateCollectible()
    {
        collectiblesCollected += 1;
        //Collectible1.sprite. = collectiblesCollected / 5;
    }

}
