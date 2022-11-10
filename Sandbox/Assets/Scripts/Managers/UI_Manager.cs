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

    private void Awake() 
    {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    // ============ SINGLETON ============

    // References
    public TMP_Text MoneyDisplay;
    public TMP_Text InteractionDisplay;
    public GameObject notificationIcon;

    [Header("Phone Panel")]
    public GameObject PhonePanel;
    public GameObject HomepagePanel, MapPanel, FastTravelPanel, MessengerPanel, MinigamePanel, MultiplayerPanel, SettingsPanel, QuitPanel;

    private void Start() 
    {
        GameManager.GetInstance().pcRef.inputAction.Player.Escape.performed += cntxt => TogglePhonePanel(!PhonePanel.activeInHierarchy);
    }

    public void TogglePhonePanel(bool state) 
    {
        PhonePanel.SetActive(state);
        GameManager.GetInstance().Pause(state);
    }

    // Phone Panel Functions
    public void ToggleMapPanel(bool state) 
    {
        MapPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
        Debug.Log("Map Panel Clicked");
    }

    public void ToggleFastTravelPanel(bool state) 
    {
        FastTravelPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }
    
    public void ToggleMessengerPanel(bool state) 
    {
        MessengerPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    public void ToggleMinigamePanel(bool state) 
    {
        MinigamePanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    public void ToggleMultiplayerPanel(bool state) 
    {
        MultiplayerPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
    }

    public void ToggleSettingsPanel(bool state) 
    {
        SettingsPanel.SetActive(state);
        HomepagePanel.SetActive(!state);
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
        notificationIcon.GetComponent<Animation>().Play("notificationFadeOut");
    }

}
