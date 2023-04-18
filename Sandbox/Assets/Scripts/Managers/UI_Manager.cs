using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

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
    public GameObject HomepagePanel, MapPanel, FastTravelPanel, MessengerPanel, MinigamePanel, HelpPanel, SettingsPanel, QuitPanel, TipPanel;
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

    [Header("Help Panel")]
    public Sprite defaultHelpPanelSprite;
    public GameObject helpList;
    public bool viewingTutorial = false; //is the player currently viewing a tutorial?

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

    [Header("Post Race Panel")]
    public GameObject PostRacePanel;
    public TMP_Text PR_TimeDisplay;
    public TMP_Text PR_HighscoreDisplay;
    public TMP_Text PR_RaceName;
    public GameObject PR_HighscoreSprite;

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

    [Header("Npc Race Panel")]
    public GameObject RacePanel;
    public TMP_Text t_raceName;
    public TMP_Text t_challenge;
    public TMP_Text t_timeToBeat;
    public TMP_Text t_bestTime;
    public List<TMP_Text> leaderboardTimes;
    public Button StartRaceButton;
    public Button ComeBackButton;

    [Header("Vending Machine Dialogue Panel")]
    public GameObject VendingDialoguePanel;
    public TMP_Text VendingMoney;
    public TMP_Text VendingOutputDisplay;
    public Button VendingYesButton;
    public Button VendingNoButton;
    public Button VendingCloseButton;

    [Header("Interaction Prompt Panel")]
    public GameObject PromptPanel;
    public TMP_Text PromptKeyDisplay;

    [Header("Chat")]
    public GameObject chatPanel;
    public Transform chatBoxContent;
    public Scrollbar verticalScroll;
    public TMP_InputField chatInputField;
    public GameObject chatMessagePrefab;

    [Header("Message of the Day")]
    public Image messageOfTheDay;
    public List<Sprite> messagesSprites;
    public GameObject motd_leftArrow;
    public GameObject motd_rightArrow;
    public GameObject motd_closeButton;
    public TMP_Text pageCount;
    public int messageIndex = 0;

    [Header("Map Panel")]
    public GameObject milkZoneMap;
    public GameObject booksZoneMap;
    public GameObject batteryZoneMap;
    public GameObject mixtapeZoneMap;
    public GameObject hardhatsZoneMap;
    public GameObject powercellZoneMap;
    public GameObject bootsZoneMap;
    public GameObject celeryZoneMap;
    public GameObject shovelZoneMap;
    public GameObject boulderZoneMap;
    public GameObject cpuZoneMap;
    public GameObject cupcakeZoneMap;
    public GameObject boneZoneMap;

    [Header("Tutorial")]
    public GameObject BeaTutorialPanel;
    public Button BeaYes;
    public Button BeaNo;
    public TMP_Text BeaText;

    private void Start() 
    {
        //InputManager.GetInput().Player.Escape.performed += cntxt => TogglePhonePanel(!PhonePanel.activeInHierarchy);
    
        questItemIcons = new GameObject[3];
        questItemIcons[0] = questItem1;
        questItemIcons[1] = questItem2;
        questItemIcons[2] = questItem3;

        EventManager.OnRaceBegin += EnableRaceTimer;
        EventManager.OnRaceEnd += DisableRaceTimer;        

        //Debug.Log(NotificationObject.gameObject.name);
    }

    // ============ PROMPT FUNCTIONS =====================

    public void TogglePrompt(bool state, string key = "") 
    {
        PromptPanel.SetActive(state);
        PromptKeyDisplay.text = key;
    }

    // ============ MAP FUNCTIONS =====================

    public void ToggleQuestZoneOnMap(int questID, bool state)
    {
        switch (questID)
        {
            case 0:
                milkZoneMap.SetActive(state);
                break;
            case 1:
                batteryZoneMap.SetActive(state);
                break;
            case 2:
                shovelZoneMap.SetActive(state);
                break;
            case 3:
                hardhatsZoneMap.SetActive(state);
                break;
            case 4:
                cupcakeZoneMap.SetActive(state);
                break;
            case 5:
                powercellZoneMap.SetActive(state);
                break;
            case 6:
                bootsZoneMap.SetActive(state);
                break;
            case 7:
                celeryZoneMap.SetActive(state);
                break;
            case 8:
                boneZoneMap.SetActive(state);
                break;
            case 9:
                mixtapeZoneMap.SetActive(state);
                break;
            case 10:
                boulderZoneMap.SetActive(state);
                break;
            case 11:
                cpuZoneMap.SetActive(state);
                break;
            case 12:
                booksZoneMap.SetActive(state);
                break;
        }
    }

    // ============ DIALOGUE FUNCTIONS =====================

    public void LoadNpcDialogue(NPC npcRef) 
    {
        // Lock the players controls        
        Controller.Local.e_State = PlayerState.Locked;
        Controller.Local.b_IsDialogue = true;

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
        Controller.Local.b_IsDialogue = false;

        // Switch Cursor Mode
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Toggle Dialogue Panel
        DialoguePanel.SetActive(false);
    }

    public void LoadRaceNpcDialogue(NPC npcRef)
    {
        // Lock the players controls        
        Controller.Local.e_State = PlayerState.Locked;
        Controller.Local.b_IsDialogue = true;

        // Move Camera to better position and look at NPC
        // CameraController.LocalCamera.LookAt(npc.gameObject);

        // Switch cursor mode
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        RacePanel.SetActive(true);
        RaceManager.GetInstance().raceList[npcRef.m_ID].raceTimes.Sort(); //sorts the race times from lowest to highest (hopefully) before they are displayed
        for(int i = 0; i < RaceManager.GetInstance().raceList[npcRef.m_ID].raceTimes.Count; i++)
        {
            leaderboardTimes[i].gameObject.SetActive(true);
            leaderboardTimes[i].text = RaceManager.GetInstance().raceList[npcRef.m_ID].raceTimes[i].ToString();
        }

        NpcData data = NpcData.Get(npcRef.m_ID);
        t_raceName.text = data.NpcName;
        t_challenge.text = data.NpcDialogue[0];
        t_timeToBeat.text = data.m_TimeToBeat;
        if(RaceManager.GetInstance().raceList[npcRef.m_ID].m_Score <= 0)
        {
            t_bestTime.text = "-";
        }
        else
        {
            t_bestTime.text = RaceManager.GetInstance().raceList[npcRef.m_ID].m_Score.ToString();
        }
        
        //insert code for times here
    }

    public void EndNpcRaceDialogue()
    {
        // Unlock Player Controls
        Controller.Local.e_State = PlayerState.Active;
        Controller.Local.b_IsDialogue = false;

        // Switch Cursor Mode
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Toggle Dialogue Panel
        RacePanel.SetActive(false);
    }

    public void LoadVendingMachineDialogue()
    {
        // Lock the players controls        
        Controller.Local.e_State = PlayerState.Locked;
        Controller.Local.b_IsDialogue = true;

        // Move Camera to better position and look at NPC
        // CameraController.LocalCamera.LookAt(npc.gameObject);

        // Switch cursor mode
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Toggle Dialogue Panel
        VendingDialoguePanel.SetActive(true);

        //Set Player Money Amount
        VendingMoney.text = "MONEY: $" + GameManager.GetInstance().pcRef.GetMoney();

        //Set text in case text was changed
        VendingOutputDisplay.text = "Buy a drink for $50?";

        //Activate yes and no button in case yes button was deactivated , turn off close button in case it was already on
        VendingYesButton.gameObject.SetActive(true);
        VendingNoButton.gameObject.SetActive(true);
        VendingCloseButton.gameObject.SetActive(false);
    }

    public void EndVendingMachineDialogue()
    {
        // Unlock Player Controls
        Controller.Local.e_State = PlayerState.Active;
        Controller.Local.b_IsDialogue = false;

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
            case 3: // Pumpkin
                CollectibleImage.sprite = Resources.Load<Sprite>("Sprites/pumpkin_icon");
                break;
            case 4: // Doggo
                CollectibleImage.sprite = Resources.Load<Sprite>("Sprites/doggo_icon");
                break;
            case 5: // Camera
                CollectibleImage.sprite = Resources.Load<Sprite>("Sprites/camera_icon");
                break;
        }
    }

    public void UpdateCollectibleAnnouncement(int ID) 
    {
        CollectibleTracker ct = GameObject.Find("CollectibleTracker").GetComponent<CollectibleTracker>();
        int collectiblesFound = ct.TotalFound(ID); // +1 bc the actually collection code gets called after announcement
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
                totalCollectibles = ct.Cans.Length;
                CollectibleAnnouncement.text = "Energy Drink!! (" + collectiblesFound + "/" + totalCollectibles + ")";
                break;
            case 3: // Pumpkin
                totalCollectibles = ct.Pumpkins.Length - 1;
                CollectibleAnnouncement.text = "Pumpkin!! (" + collectiblesFound + "/" + totalCollectibles + ")";
                break;
            case 4: // Doggo
                totalCollectibles = ct.Doggos.Length - 1;
                CollectibleAnnouncement.text = "Doggo!! (" + collectiblesFound + "/" + totalCollectibles + ")";
                break;
            case 5: // Camera
                totalCollectibles = ct.Cameras.Length - 1;
                CollectibleAnnouncement.text = "Handheld Camera!! (" + collectiblesFound + "/" + totalCollectibles + ")";
                break;
        }
    }

    public void UpdateCollectibleUI(int ID)
    {
        CollectibleTracker ct = GameObject.Find("CollectibleTracker").GetComponent<CollectibleTracker>();
        int collectiblesFound = ct.TotalFound(ID); // +1 bc the actually collection code gets called after announcement
        int totalCollectibles = 0;

        switch (ID)
        {
            default: // ERROR
                CollectibleAnnouncement.text = "UNKNOWN EXCEPTION";
                break;
            case 0: // Rex
                collectibleBoxUI[0].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/3";
                break;
            case 1: // Mbear
                collectibleBoxUI[1].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/3";
                break;
            case 2: // Can
                collectibleBoxUI[2].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/7";
                break;
            case 3: // Pumpkin
                collectibleBoxUI[3].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/3";
                break;
            case 4: // Doggo
                collectibleBoxUI[4].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/3";
                break;
            case 5: // Camera
                collectibleBoxUI[5].GetComponent<CollectibleInfo>().c_progress.text = collectiblesFound + "/3";
                break;
        }
    }

    // ============ POST RACE PANEL FUNCTIONS =====================

    public IEnumerator TogglePostRacePanel(bool state, float duration = 0) 
    {   
        if (state == true) {
            PostRacePanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            PostRacePanel.SetActive(false);
        } else 
        {
            yield return null;
            PostRacePanel.SetActive(false);
        }        
    }

    public void UpdatePostRaceTime(float time) 
    {
        PR_TimeDisplay.text = time.ToString();
    }

    public void UpdatePostRaceHighscore(float highscore)
    {
        PR_HighscoreDisplay.text = highscore.ToString();
    }

    public void UpdatePostRaceName(string result)
    {
        PR_RaceName.text = result;
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
        if (Controller.Local.b_IsDialogue && state == true) return;        

        Controller.Local.e_State = (state) ? PlayerState.Locked : PlayerState.Active;
        PhonePanel.SetActive(state);
        phoneAnimator.SetBool("PhoneOpen", true);
        GameManager.GetInstance().Pause(state);

        //check if a race is active
        if (RaceManager.GetInstance() != null)
        {
            if (RaceManager.GetInstance().m_RaceActive == true)
            {
                ToggleQuitPanel(true);
            }
            else
            {
                ToggleQuitPanel(false);
            }
        }

        else
        {
            ToggleQuitPanel(false);
        }

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

    public void ToggleHelpPanel(bool state) 
    {
        HelpPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        SearchBar.gameObject.SetActive(!state);
    }

    public void ToggleHelpList() //goes back to the tutorial/help list
    {
        HelpPanel.GetComponent<Image>().sprite = defaultHelpPanelSprite; //set the background of the help panel back to the default one
        helpList.SetActive(true); //reactivate the help list
        viewingTutorial = false; //set to false since player is no longer viewing a singular tutorial
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
            cashTracker.gameObject.SetActive(true);
        }
        else if(viewingTutorial == true)
        {
            ToggleHelpList();
        }
        else if(QuitPanel.activeSelf == true) //if the quit panel is active turn off the phone when the back or home button is pressed
        {
            TogglePhonePanel(false);
        }
        else
        {
            MapPanel.SetActive(!state);
            FastTravelPanel.SetActive(!state);
            MessengerPanel.SetActive(!state);
            MinigamePanel.SetActive(!state);
            HelpPanel.SetActive(!state);
            SettingsPanel.SetActive(!state);
            TipPanel.SetActive(!state);
            QuitPanel.SetActive(!state);
            HomepagePanel.SetActive(state);
            SearchBar.gameObject.SetActive(state);
        }        
    }

    public void QuitToMenu()
    {
        NpcData.FlushData();
        SceneManager.LoadScene(1);        
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
        if (RaceManager.GetInstance() != null)
        {
            if (RaceManager.GetInstance().m_RaceActive == true)
            {
                return;
            }
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
        if (RaceManager.GetInstance() != null)
        {
            if (RaceManager.GetInstance().m_RaceActive == true)
            {
                return;
            }
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

    // ============ MESSAGE OF THE DAY FUNCTIONS =====================
    public void NextMessage() //right arrow pressed
    {
        messageIndex++;
        if(messageIndex == messagesSprites.Count -1)
        {
            motd_rightArrow.gameObject.SetActive(false);
        }
        else
        {
            motd_rightArrow.gameObject.SetActive(true);
        }
        messageOfTheDay.sprite = messagesSprites[messageIndex];

        motd_leftArrow.gameObject.SetActive(true);

        int pageNum = messageIndex + 1;
        pageCount.text = pageNum.ToString() + "/" + messagesSprites.Count.ToString();
    }

    public void PreviousMessage() //left arrow pressed
    {        
        messageIndex--;
        if(messageIndex == 0)
        {
            motd_leftArrow.gameObject.SetActive(false);
        }
        else
        {
            motd_leftArrow.gameObject.SetActive(true);
        }
        messageOfTheDay.sprite = messagesSprites[messageIndex];

        motd_rightArrow.gameObject.SetActive(true);

        int pageNum = messageIndex + 1;
        pageCount.text = pageNum.ToString() + "/" + messagesSprites.Count.ToString();
    }

    // ============ RACE =====================

    public void QuitRace()
    {
        if(RaceManager.GetInstance().m_RaceActive == true)
        {
            EventManager.OnRaceEnd?.Invoke(true);
            TogglePhonePanel(false);
        }
    }

    // ============ TUTORIAL =====================
    public void ToggleBeaTutorialPanel(bool state)
    {
        BeaTutorialPanel.SetActive(state);
    }
}
