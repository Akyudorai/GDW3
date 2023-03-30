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

    [Header("Screen Panel")]
    public GameObject ScreenPanel;
    public TMP_Text RaceTimer;

    [Header("Phone Panel")]
    public GameObject PhonePanel;
    public GameObject HomepagePanel, MapPanel, FastTravelPanel, MessengerPanel, MinigamePanel, MultiplayerPanel, SettingsPanel, QuitPanel, TipPanel;
    public Animator phoneAnimator;
    public TextMeshProUGUI cashTracker; //temporary cash ui element

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
    public TextMeshProUGUI questNpcName;
    public GameObject questItem1;
    public GameObject questItem2;
    public GameObject questItem3;
    public GameObject questLogItem; //the ui element that stores the quest info on the quest screen.
    public GameObject questContentPanel;
    public GameObject raceContentPanel;
    public GameObject questPfp;
    public GameObject questStatusImg;
    public GameObject[] questItemIcons;
    public List<Sprite> questLabels;

    [Header("Fast Travel Panel")]
    public int stopIndex;
    public TextMeshProUGUI destination;

    [Header("Tip Panel")]
    public List<string> tips;

    [Header("Settings Panel")]
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Notification")]
    public GameObject NotificationObject;
    public Image notificationIcon;
    public TextMeshProUGUI notificationText;
    public Sprite questSprite;
    public GameObject newQuestNotifactionIcon;
    public GameObject questCompleteNotificationIcon;
    public GameObject moneyEarnedNotificationIcon;

    [Header("Other Components")]
    public TMP_Text SpeedTracker;
    public TMP_Text CountdownDisplay;

    [Header("Postgame Panel")]
    public GameObject PostgamePanel;
    public TMP_Text PG_TimeDisplay;
    public TMP_Text PG_HighscoreDisplay;
    public TMP_Text PG_PositionDisplay;

    [Header("Collectible Panel")]
    public GameObject CollectiblePanel;
    public Image CollectibleImage;
    public TMP_Text CollectibleAnnouncement;
    public List<GameObject> collectibleBoxUI;

    [Header("Npc Dialogue Panel")]    
    public GameObject DialoguePanel;
    public Image DialogueBox;
    public TMP_Text DialogueOutputDisplay;
    public Button YesDialogueButton;
    public Button NoDialogueButton;

    [Header("Vending Machine Dialogue Panel")]
    public GameObject VendingDialoguePanel;
    public TMP_Text VendingMoney;
    public TMP_Text VendingOutputDisplay;
    public Button VendingYesButton;
    public Button VendingNoButton;

    [Header("Interaction Prompt Panel")]
    public GameObject PromptPanel;
    public TMP_Text PromptKeyDisplay;

    [Header("Chat")]
    public GameObject chatPanel;
    public Transform chatBoxContent;
    public Scrollbar verticalScroll;
    public TMP_InputField chatInputField;
    public GameObject chatMessagePrefab;


    private void Start() 
    {
        //InputManager.GetInput().Player.Escape.performed += cntxt => TogglePhonePanel(!PhonePanel.activeInHierarchy);
    
        questItemIcons = new GameObject[3];
        questItemIcons[0] = questItem1;
        questItemIcons[1] = questItem2;
        questItemIcons[2] = questItem3;

        EventManager.OnRaceBegin += EnableRaceTimer;
        EventManager.OnRaceEnd += DisableRaceTimer;

        EventManager.OnCollectibleFound += UpdateCollectibleImage;
        EventManager.OnCollectibleFound += UpdateCollectibleAnnouncement;
        EventManager.OnCollectibleFound += UpdateCollectibleUI;

        //Debug.Log(NotificationObject.gameObject.name);
    }

    // ============ PROMPT FUNCTIONS =====================

    public void TogglePrompt(bool state, string key = "") 
    {
        PromptPanel.SetActive(state);
        PromptKeyDisplay.text = key;
    }

    // ============ DIALOGUE FUNCTIONS =====================

    public void LoadNpcDialogue(NPC npcRef) 
    {
        // Lock the players controls        
        Controller.Local.e_State = PlayerState.Locked;

        // Move Camera to better position and look at NPC
        // CameraController.LocalCamera.LookAt(npc.gameObject);
        
        // Switch cursor mode
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Toggle Dialogue Panel
        DialoguePanel.SetActive(true); 
        DialogueBox.sprite = npcRef.DialogueImage;   

        if (npcRef.m_Data.m_Type == NpcType.Standard) {
            YesDialogueButton.gameObject.SetActive(false);
        } 
        else if (npcRef.m_Data.m_Type == NpcType.Quest_Giver) 
        {
            if (QuestManager.GetInstance().questList[npcRef.m_QuestID].m_Collected == true)
            {
                YesDialogueButton.gameObject.SetActive(false);
            } else {
                YesDialogueButton.gameObject.SetActive(true);
                YesDialogueButton.GetComponent<Image>().sprite = npcRef.DialogueYesImage;
            }
        }
        else {
            // ADD AN IF STATEMENT TO CHECK IF RACE IS ACTIVE OR NOT
            YesDialogueButton.gameObject.SetActive(true);
            YesDialogueButton.GetComponent<Image>().sprite = npcRef.DialogueYesImage;                     
        }
                    
        // Create an animated typing effect on the dialogue box
        NpcData data = NpcData.Get(npcRef.m_ID);
        //DialogueNameDisplay.text = data.NpcName;
        DialogueOutputDisplay.text = data.NpcDialogue[0];

        // Once the typing is complete, reveal list of interaction options
    }

    public void EndNpcDialogue() 
    {
        // Unlock Player Controls
        Controller.Local.e_State = PlayerState.Active;

        // Switch Cursor Mode
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Toggle Dialogue Panel
        DialoguePanel.SetActive(false);
    }

    public void LoadVendingMachineDialogue()
    {
        // Lock the players controls        
        Controller.Local.e_State = PlayerState.Locked;

        // Move Camera to better position and look at NPC
        // CameraController.LocalCamera.LookAt(npc.gameObject);

        // Switch cursor mode
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Toggle Dialogue Panel
        VendingDialoguePanel.SetActive(true);

        //Set Player Money Amount
        VendingMoney.text = "MONEY: $" + GameManager.GetInstance().pcRef.GetMoney();


    }

    public void EndVendingMachineDialogue()
    {
        // Unlock Player Controls
        Controller.Local.e_State = PlayerState.Active;

        // Switch Cursor Mode
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Toggle Vending Machine Dialogue Panel
        VendingDialoguePanel.SetActive(false);
    }

    // ============ COLLECTIBLE FUNCTIONS =====================

    public IEnumerator ToggleCollectiblePanel(bool state, float duration = 0) 
    {
        if (state == true) 
        {
            CollectiblePanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            CollectiblePanel.SetActive(false);            
        } else 
        {
            yield return null;
            CollectiblePanel.SetActive(false);
        }
    }

    public void UpdateCollectibleImage(int ID) 
    {
        switch (ID) 
        {
            default: // ERROR
                CollectibleImage.sprite = null;
                break;
            case 0: // Rex
                CollectibleImage.sprite = Resources.Load<Sprite>("Sprites/rex_icon");
                break;
            case 1: // Mbear
                CollectibleImage.sprite = Resources.Load<Sprite>("Sprites/mbear_icon");
                break;
            case 2: // Can
                CollectibleImage.sprite = Resources.Load<Sprite>("Sprites/can_icon");
                break;
        }
    }

    public void UpdateCollectibleAnnouncement(int ID) 
    {
        CollectibleTracker ct = GameObject.Find("CollectibleTracker").GetComponent<CollectibleTracker>();
        int collectiblesFound = ct.TotalFound(ID) /*+ 1*/; // +1 bc the actually collection code gets called after announcement
        int totalCollectibles = 0;

        switch (ID) 
        {
            default: // ERROR
                CollectibleAnnouncement.text = "UNKNOWN EXCEPTION";
                break;
            case 0: // Rex
                
                totalCollectibles = ct.Rexs.Length - 1;
                CollectibleAnnouncement.text = "Tinysaurous Rex!! ("+collectiblesFound+"/"+totalCollectibles+")";
                break;
            case 1: // Mbear
                totalCollectibles = ct.Mbears.Length - 1;
                CollectibleAnnouncement.text = "Mithunan Bear Plushie!! ("+collectiblesFound+"/"+totalCollectibles+")";
                break;
            case 2: // Can
                totalCollectibles = ct.Cans.Length - 1;
                CollectibleAnnouncement.text = "Energy Drink!! (" + collectiblesFound + "/" + totalCollectibles + ")";
                break;
        }
    }

    public void UpdateCollectibleUI(int ID)
    {
        CollectibleTracker ct = GameObject.Find("CollectibleTracker").GetComponent<CollectibleTracker>();
        int collectiblesFound = ct.TotalFound(ID) + 1; // +1 bc the actually collection code gets called after announcement
        int totalCollectibles = 0;

        switch (ID)
        {
            default: // ERROR
                CollectibleAnnouncement.text = "UNKNOWN EXCEPTION";
                break;
            case 0: // Rex
                collectibleBoxUI[0].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/2";
                break;
            case 1: // Mbear
                collectibleBoxUI[1].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/2";
                break;
            case 2: // Can
                collectibleBoxUI[2].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/7";
                break;
        }
    }

    // ============ POSTGAME PANEL FUNCTIONS =====================

    public IEnumerator TogglePostgamePanel(bool state, float duration = 0) 
    {   
        if (state == true) {
            PostgamePanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            PostgamePanel.SetActive(false);
        } else 
        {
            yield return null;
            PostgamePanel.SetActive(false);
        }        
    }

    public void UpdatePostgameTime(float time) 
    {
        PG_TimeDisplay.text = "YOUR TIME: " + time;
    }

    public void UpdatePostgameHighscore(float highscore)
    {
        PG_HighscoreDisplay.text = "HIGHSCORE: " + highscore;
    }

    public void UpdatePostgamePosition(string result)
    {
        PG_PositionDisplay.text = result;
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
        phoneAnimator.SetBool("PhoneOpen", true);
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
        if(questInfoPanel.activeSelf == true)
        {
            questInfoPanel.SetActive(false);
            questListPanel.SetActive(true);
        }
        else
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

    public void UpdateNpcName(string _name)
    {
        questNpcName.text = _name;
    }

    public void UpdateQuestPfp(Sprite _pfp)
    {
        questPfp.GetComponent<Image>().sprite = _pfp;
    }

    public void UpdateQuestStatusImg(Sprite _img)
    {
        questStatusImg.GetComponent<Image>().sprite = _img;
    }

    public void ActivateToggle()
    {
        //QuestManager.GetInstance().ActivateQuest(QuestManager.GetInstance().questList[QuestManager.GetInstance().selectedQuest.questId]); //get the quest id from the quest data display object
    }

    public void QuestActivateToggle() //testing quest activate and deactivate system
    {
        QuestManager.GetInstance().ActivateDeactivateQuest(QuestManager.GetInstance().questList[QuestManager.GetInstance().selectedQuest.questId]);
    }

    // ============ FAST TRAVEL PANEL FUNCTIONS =====================

    public void NextStop()
    {
        if(RaceManager.GetInstance().m_RaceActive == true)
        {
            return;
        }
        List<Transform> points = SpawnPointManager.GetInstance().SpawnPoints;
        stopIndex++;
        if (stopIndex >= points.Count)
        {
            stopIndex = 0;
        }
        destination.text = points[stopIndex].gameObject.name;
    }

    public void PreviousStop()
    {
        if (RaceManager.GetInstance().m_RaceActive == true)
        {
            return;
        }
        List<Transform> points = SpawnPointManager.GetInstance().SpawnPoints;
        stopIndex--;
        if (stopIndex < 0)
        {
            stopIndex = points.Count - 1;
        }
        destination.text = points[stopIndex].gameObject.name;
    }

    public void CallTaxi()
    {
        GameManager.GetInstance().RespawnPlayer(stopIndex);

        //Play the Fast Travel Sound Effect
        FMOD.Studio.EventInstance fastTravelSfx = SoundManager.CreateSoundInstance(SoundFile.FastTravel);
        fastTravelSfx.start();
        fastTravelSfx.release();
    }

    // ============ Collectibles PANEL FUNCTIONS =====================

    // ============ Tip PANEL FUNCTIONS =====================
    public string CreateNewTip()
    {
        int newTipIndex = Random.Range(0, tips.Count);
        
        return tips[newTipIndex];
    }

    // ============ OTHER COMPONENTS =====================

    public void UpdateSpeedTracker(float speed) 
    {
        SpeedTracker.text = speed.ToString("F2");
    }

    public void ToggleCountdown(bool state) 
    {
        CountdownDisplay.enabled = state;
    }

    public void UpdateCountdown(string value) 
    {
        CountdownDisplay.text = value;
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

    public void SendNotification(string _text, Sprite _sprite)
    {
        notificationText.text = _text;
        notificationIcon.sprite = _sprite;
        NotificationObject.GetComponent<Notification>().notificationAnimator.SetTrigger("PlayNotification");
        PlayPhoneNotification();
    }

    public void SendNewQuestNotification()
    {
        newQuestNotifactionIcon.GetComponent<Animator>().SetTrigger("t_NewQuest");
    }

    public void SendQuestCompleteNotification()
    {
        questCompleteNotificationIcon.GetComponent<Animator>().SetTrigger("t_QuestComplete");
    }

    public void SendMoneyEarnedNotification()
    {
        moneyEarnedNotificationIcon.GetComponent<Animator>().SetTrigger("t_MoneyEarned");
    }

    // ============ SETTINGS PANEL FUNCTIONS =====================

    public void AdjustMasterVolume() 
    {
        SaveManager.Settings.Master_Volume = masterVolumeSlider.value;
        SoundManager.GetInstance().UpdateSoundSettings();
    }

    public void AdjustBGMVolume()
    {
        SaveManager.Settings.BGM_Volume = bgmVolumeSlider.value;
        SoundManager.GetInstance().UpdateSoundSettings();
    }

    public void AdjustSoundEffectVolume()
    {
        SaveManager.Settings.SFX_Volume = sfxVolumeSlider.value;
        SoundManager.GetInstance().UpdateSoundSettings();
    }

    public void PlayAppHighlight()
    {
        FMOD.Studio.EventInstance appHighlightSFX = SoundManager.CreateSoundInstance(SoundFile.AppHighlight);
        appHighlightSFX.start();
        appHighlightSFX.release();
    }

    public void PlayAppClick()
    {
        FMOD.Studio.EventInstance appClickSFX = SoundManager.CreateSoundInstance(SoundFile.AppClick);
        appClickSFX.start();
        appClickSFX.release();
    }

    public void PlayPhoneNotification()
    {
        FMOD.Studio.EventInstance phoneNotiSFX = SoundManager.CreateSoundInstance(SoundFile.PhoneNotification);
        phoneNotiSFX.start();
        phoneNotiSFX.release();
    }
    
}
