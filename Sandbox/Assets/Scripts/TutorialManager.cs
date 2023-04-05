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
    
        yield return new WaitForSeconds((float)cinematics.Sequences[0].asset.duration);

        GameManager.GetInstance().pcRef.SetPlayerState(PlayerState.Active);
        GameManager.GetInstance().pcRef.animationHandler.animator.SetTrigger("ExitCinematic");
    }
}
