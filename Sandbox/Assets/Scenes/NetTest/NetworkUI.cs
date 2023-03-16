using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    public static NetworkUI instance;

    [Header("Menu")]
    public GameObject startMenu;
    public TMP_InputField usernameInputField; 
    public TMP_InputField ipInputField;
    public GameObject menuCamera;

    [Header("Chat")]
    public GameObject chatPanel;
    public Transform chatBoxContent;
    public Scrollbar verticalScroll;
    public TMP_InputField chatInputField;
    public GameObject chatMessagePrefab;

    public void Awake() 
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) 
        {
            Debug.LogWarning("Instance already exists, destrying objects!");
            Destroy(this);
        }
    }

    public void ConnectToServer() 
    {
        startMenu.SetActive(false);
        menuCamera.SetActive(false);
        usernameInputField.interactable = false;
        ipInputField.interactable = false;
        Client.instance.ConnectToServer();

        chatPanel.SetActive(true);        
    }

    public void ReceiveMessage(string _msg) 
    {
        GameObject msg = Instantiate(chatMessagePrefab, chatBoxContent);
        msg.GetComponent<TMP_Text>().text = _msg;          
    }

    public void SendChatMessage()
    {
        ClientSend.ChatMessage();
        chatInputField.text = "";
    }
}
