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
    

    public void InitializeRace(PlayerController pcRef, int raceID)
    {
        pcRef.gameObject.transform.position = raceList[raceID].m_StartPoint.position;
        pcRef.gameObject.transform.rotation = raceList[raceID].m_StartPoint.rotation;

        m_RaceTimer = 0.0f;
        finishColliderReference = Instantiate(finishColliderPrefab, raceList[raceID].m_EndPoint.position, Quaternion.identity);
        finishColliderReference.GetComponent<FinishLine>().OnContact += RaceComplete;
        activeRaceID = raceID;
        m_RaceActive = true;

    }

    private void Update() 
    {
        if (m_RaceActive) 
        {
            m_RaceTimer += Time.deltaTime;
        }
    }

    public void RaceComplete()
    {
        m_RaceActive = false;
        raceList[activeRaceID].m_Score = m_RaceTimer;
        activeRaceID = -1;        
    }
}
