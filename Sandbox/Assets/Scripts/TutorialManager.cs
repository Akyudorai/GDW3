using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public CinematicsManager cinematics;   

    private void Awake()
    {
        EventManager.OnPlayerStartSingleplayer += PlayIntro;        
    }

    private void OnDestroy()
    {
        EventManager.OnPlayerStartSingleplayer -= PlayIntro;
    }

    public void PlayIntro()
    {
        StartCoroutine(PlayIntroCinematic());
    }

    private IEnumerator PlayIntroCinematic()
    {
        GameManager.GetInstance().pcRef.animationHandler.animator.SetTrigger("PlayCinematic");
        cinematics.Play(0);
        GameManager.GetInstance().pcRef.SetPlayerState(PlayerState.Cinematic);
        
        // Pause the BGM and begin playing the cinematic sound file
        SoundManager.GetInstance().backgroundMusic.setPaused(true);
        SoundManager.GetInstance().cityAmbience.setPaused(false);
        FMOD.Studio.EventInstance cinematicSFX = SoundManager.CreateSoundInstance(SoundFile.IntroCinematic);
        cinematicSFX.start();
        cinematicSFX.release();

        yield return new WaitForSeconds((float)cinematics.Sequences[0].asset.duration);

        GameManager.GetInstance().pcRef.SetPlayerState(PlayerState.Active);
        GameManager.GetInstance().pcRef.animationHandler.animator.SetTrigger("ExitCinematic");
        SoundManager.GetInstance().backgroundMusic.setPaused(false);
        SoundManager.GetInstance().backgroundMusic.setTimelinePosition(0);                
    }
}
