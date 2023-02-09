using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

// -- NOTE -- Cinematics Manager is not included in the GameLoader.  We may want to change this at a later time
public class CinematicsManager : MonoBehaviour
{
    [Header("Camera References")]
    //public GameObject CinematicCamera;
    public GameObject MainCamera;
    public GameObject PlayerView;
    public GameObject CinematicView;


    [Header("Director")]
    public PlayableDirector Director;

    [Header("Debugging")]
    public int currentCinematicIndex = -1;
    public bool playOnAwake = false;
    
    [Header("Paths")]
    public CinemachineVirtualCamera VirtualViewCamera;
    public Animator VirtualViewAnimator;
    public CinemachineDollyCart TargetDolly;
    public Animator VirtualTargetAnimator;   

    [Header("Sequences")]
    public List<CinematicSequence> Sequences = new List<CinematicSequence>();

    private void Start() 
    {
        if (playOnAwake && currentCinematicIndex != -1) {
            Play(currentCinematicIndex);
        } 
        
    }

    public void SetViewTarget(GameObject target) 
    {
        VirtualViewCamera.LookAt = target.transform;
    }

    public void SetViewNode(int index) 
    {
        VirtualViewCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = index;
    }

    public void Play(int index) 
    {        
        CinematicSequence sequence = Sequences[index]; 
        currentCinematicIndex = index;

        // Set the playable sequence in the director
        Director.playableAsset = sequence.asset;

        // Get the timeline asset from the playable sequence
        var timelineAsset = Director.playableAsset as TimelineAsset;        

        // Assign the animators for the virtual cameras within the timeline asset
        Director.SetGenericBinding(timelineAsset.GetOutputTrack(0), VirtualViewAnimator);     // 0 is always the View Camera
        Director.SetGenericBinding(timelineAsset.GetOutputTrack(1), VirtualTargetAnimator);     // 1 is always the Target Camera

        // Set the Paths for the TargetAnimatorDolly and ViewVirtualCamera
        VirtualViewCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = sequence.viewPath;
        VirtualViewCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0.0f;
        TargetDolly.m_Path = sequence.targetPath;
        TargetDolly.m_Position = 0;

        // Toggle the Player Camera off and the Cinematics Camera on        
        PlayerView.SetActive(true);
        CinematicView.SetActive(true);

        // Begin the cinematic sequence
        Director.Play();
        EventManager.OnCinematicBegin?.Invoke(index);

        // Begin a coroutine to handle completion event of the cinematic
        StartCoroutine(WaitForSequenceToFinish(true, (float)sequence.asset.duration));
    }

    public IEnumerator WaitForSequenceToFinish(bool state, float waitDuration = 0f) 
    {
        if (state) 
        {               
            yield return new WaitForSeconds(waitDuration); 
            CinematicView.SetActive(false);
            PlayerView.SetActive(true);
            EventManager.OnCinematicEnd?.Invoke(currentCinematicIndex);
            currentCinematicIndex = -1;
        }
    }


}
