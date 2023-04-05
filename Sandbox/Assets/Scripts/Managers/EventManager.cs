using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager 
{   
    // -- DELEGATES --
    public delegate void OnVoidDelegate();
    public delegate void OnIntDelegate(int i);
    public delegate void OnBoolDelegate(bool b);
    
    // -- GAME EVENTS
    public static OnIntDelegate OnRaceBegin;
    public static OnBoolDelegate OnRaceEnd;
    public static OnVoidDelegate OnDayBegin;
    public static OnVoidDelegate OnNightBegin;
    public static OnVoidDelegate OnPlayerJump;
    public static OnVoidDelegate OnPlayerLanding;
    public static OnIntDelegate OnNpcInteract;
    public static OnVoidDelegate OnWallRunBegin;
    public static OnBoolDelegate OnWallRunEnd;
    public static OnIntDelegate OnQuestBegin;
    public static OnIntDelegate OnQuestComplete;
    public static OnIntDelegate OnQuestForfeit; 
    public static OnIntDelegate OnCollectibleFound;
    public static OnIntDelegate OnCinematicBegin;
    public static OnIntDelegate OnCinematicEnd;
    public static OnVoidDelegate OnPlayerStartSingleplayer;


    // -- TRANSITION EVENTS
    public static OnVoidDelegate OnGameLoaderComplete;
    
}
