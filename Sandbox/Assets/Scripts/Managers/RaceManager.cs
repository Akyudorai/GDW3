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

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // -- END OF SINGLETON --

    [Header("Race Data")]
    public GameObject finishColliderPrefab;
    public List<RaceData> raceList = new List<RaceData>();    

    [Header("Race Variables")]
    public int activeRaceID = -1;
    public float m_RaceTimer;
    public bool m_RaceActive;
    public GameObject finishColliderReference;
    
    private void Start() 
    {
        EventManager.OnRaceEnd += RaceComplete;

        for (int i = 0; i < raceList.Count; i++) 
        {
            LoadScore(i);
        }
    }

    public void InitializeRace(PlayerController pcRef, int raceID)
    {
        pcRef.gameObject.transform.position = raceList[raceID].m_StartPosition;
        pcRef.gameObject.transform.rotation = Quaternion.Euler(raceList[raceID].m_StartRotation);

        m_RaceTimer = 0.0f;
        finishColliderReference = Instantiate(finishColliderPrefab, raceList[raceID].m_EndPosition, Quaternion.identity);
        //finishColliderReference.GetComponent<FinishLine>().OnContact += RaceComplete;
        activeRaceID = raceID;
        m_RaceActive = true;
    }

    private void Update() 
    {
        if (m_RaceActive) 
        {
            m_RaceTimer += Time.deltaTime;
            UI_Manager.GetInstance().UpdateRaceTimer(m_RaceTimer);
        }
    }

    public void RaceComplete(bool isForfeit)
    {
        m_RaceActive = false;
        
        if (!isForfeit) {            
            SaveScore(activeRaceID, m_RaceTimer);
        }
        
        activeRaceID = -1;    
            
    }

    public void SaveScore(int id, float time) 
    {
        if (time <= raceList[id].m_Score) 
        {     
            raceList[activeRaceID].m_Score = m_RaceTimer;
            PlayerPrefs.SetFloat("RaceScore["+id+"]", time);
            PlayerPrefs.Save();
            Debug.Log("New Top Score: " + time);
        } else 
        {
            Debug.Log("Your score: " + time + " | Top score: " + raceList[id].m_Score);
        }
    }

    public void LoadScore(int id) 
    {        
        raceList[id].m_Score = PlayerPrefs.GetFloat("RaceScore["+id+"]");
    }
}
