using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{    
    public int citySceneIndex = 3;
    public int tutorialSceneIndex = 2;
    public int netCityIndex = 4;

    [Header("Login Panel")]
    public GameObject loginPanel;
    public TMP_InputField usernameInput;

    [Header("Menu Panel")]
    public GameObject menuPanel;
    public GameObject menuOptions;

    [Header("Play Panel")]
    public GameObject playPanel;

    [Header("Multiplayer Panel")]
    public GameObject multiplayerPanel;
    public GameObject multiplayerButton;

    [Header("Settings Panel")]
    public GameObject settingsPanel;
    

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (PlayerIdentity.Username == "") LoginPanel(true);
        else MenuPanel(true);
    }

    private void Update()
    {
        multiplayerButton.SetActive(Client.isConnected);
    }

    public void LoginPanel(bool state)
    {
        loginPanel.SetActive(state);
    }

    public void ConnectToServer()
    {
        // Initialize Player
        PlayerIdentity.NetID = null;
        PlayerIdentity.Username = usernameInput.text;

        Client.instance.ConnectToServer();
    }

    public void MenuPanel(bool state)
    {
        menuOptions.SetActive(state);
    }

    public void PlayPanel(bool state)
    {
        playPanel.SetActive(state);
    }

    public void Play() 
    {   
        int index = ((SaveManager.Save.TutorialComplete) ? citySceneIndex : tutorialSceneIndex);
        if (!SaveManager.Save.TutorialComplete) SelectCharacter(1);
        SceneManager.LoadScene(index);
    }    

    public void SelectCharacter(int id)
    {
        PlayerIdentity.Settings.Character = id;
    }

    public void MultiplayerPanel(bool state)
    {
        multiplayerPanel.SetActive(state);
    }

    public void PlayMultiplayer()
    {
        SceneManager.LoadScene(netCityIndex);
    }

    public void SettingsPanel(bool state)
    {
        settingsPanel.SetActive(state);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
