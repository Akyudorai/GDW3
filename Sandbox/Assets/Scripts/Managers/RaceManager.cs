using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    // -- SINGLETON --
    private static RaceManager instance;
    public static RaceManager GetInstance() 
    {
        return instance;
    }

    // -- END OF SINGLETON --

    [Header("Universal Values")]
    public float m_Timer;
    public bool b_Pregame;

    [Header("Race Data")]
    public List<RaceData> raceList = new List<RaceData>();    
    
    [Header("Race Variables")]
    public int activeRaceID = -1;    
    public bool m_RaceActive;

    // Initialization
    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } 

        instance = this;       

        EventManager.OnRaceEnd += RaceComplete;
        DontDestroyOnLoad(this.gameObject);
    }

    public void InitializeRace(PlayerController pcRef, int raceID)
    {
        // Initialize the appropriate Waypoint System based on index
        WaypointManager wpm = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        WaypointSystem wps = wpm.GetRaceWPS(raceList[raceID].WPS_Index);        
        wpm.InitializeRaceWPS(raceList[raceID].WPS_Index);

        // Zero out player motion
        pcRef.v_HorizontalVelocity = Vector3.zero;
        pcRef.v_VerticalVelocity = Vector3.zero;
        pcRef.rigid.velocity = Vector3.zero;        
        
        // Set player position and orientation
        pcRef.gameObject.transform.position = wps.Beginning.position;
        pcRef.mesh.transform.rotation = wps.Beginning.rotation;
        pcRef.camera_pivot.transform.rotation = wps.Beginning.rotation;

        
        // Queue the Countdown Timer
        m_RaceActive = true;
        activeRaceID = raceID;
        EventManager.OnCinematicEnd += BeginCountdown;
    }

    // For OnCinematicEnd Event
    private void BeginCountdown(int CinematicID)
    {
        StartCoroutine(Countdown(PlayerController.LocalPlayer, activeRaceID));
        EventManager.OnCinematicEnd -= BeginCountdown;        
    }

    public IEnumerator Countdown(PlayerController pcRef, int ID) 
    {        
        pcRef.SetPlayerState(PlayerState.Locked);
        b_Pregame = true;
        m_Timer = 0.0f;        

        // Update UI
        UI_Manager.GetInstance().UpdateCountdown("");
        UI_Manager.GetInstance().ToggleCountdown(true);
        
        yield return new WaitForSeconds(2.0f);

        // Play Countdown SFX
        FMOD.Studio.EventInstance countdownSFX = SoundManager.CreateSoundInstance(SoundFile.RaceCountdown);
        countdownSFX.start();
        countdownSFX.release(); 

        // 3
        UI_Manager.GetInstance().UpdateCountdown("3");
        yield return new WaitForSeconds(1.0f);
        // 2
        UI_Manager.GetInstance().UpdateCountdown("2");
        yield return new WaitForSeconds(1.0f);
        // 1
        UI_Manager.GetInstance().UpdateCountdown("1");
        yield return new WaitForSeconds(1.0f);
        UI_Manager.GetInstance().UpdateCountdown("GO!");
        
        // Begin Race    
        b_Pregame = false;  
        pcRef.SetPlayerState(PlayerState.Active);

        // Play Race Start SFX
        FMOD.Studio.EventInstance startSFX = SoundManager.CreateSoundInstance(SoundFile.RaceStart);
        startSFX.start();
        startSFX.release(); 

        if (m_RaceActive) EventManager.OnRaceBegin?.Invoke(ID);      

        yield return new WaitForSeconds(1.0f);
        UI_Manager.GetInstance().ToggleCountdown(false);

    }

    private void Update() 
    {        
        if (m_RaceActive && !b_Pregame) 
        {
            m_Timer += Time.deltaTime;
            UI_Manager.GetInstance().UpdateRaceTimer(m_Timer);
        }
    }

    public void RaceComplete(bool isForfeit)
    {        
        if (!isForfeit) {            
            SaveScore(activeRaceID, m_Timer);
        
            // Play Race Complete SFX
            FMOD.Studio.EventInstance finishSFX = SoundManager.CreateSoundInstance(SoundFile.RaceFinish);
            finishSFX.start();
            finishSFX.release(); 
        } else 
        {
            Debug.Log("Race has been forfeited");
        }

        StartCoroutine(UI_Manager.GetInstance().TogglePostgamePanel(true, 3));
        UI_Manager.GetInstance().UpdatePostgameTime(m_Timer);
        UI_Manager.GetInstance().UpdatePostgameHighscore(raceList[activeRaceID].m_Score);        
        UI_Manager.GetInstance().UpdatePostgamePosition(((isForfeit) ? "Boo!" : "Yay!"));

        m_RaceActive = false;   
        activeRaceID = -1;                
    }

    public void SaveScore(int id, float time) 
    {
        if (m_RaceActive) 
        {
            if (time <= raceList[id].m_Score || raceList[id].m_Score <= 0) 
            {     
                raceList[activeRaceID].m_Score = m_Timer;
                SaveManager.GetInstance().SaveFile();            
                Debug.Log("New Top Score: " + time);
            } else 
            {
                Debug.Log("Your score: " + time + " | Top score: " + raceList[id].m_Score);
            }
        }

        else 
        {
            Debug.Log("No active race!?");
        }
    }
}
