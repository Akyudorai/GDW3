using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    // PACKET SENDERS
    // ----------------------------------------------------------------------------

    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet) 
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    // ----------------------------------------------------------------------------
    // PACKET SENDING FUNCTIONALITY
    // ----------------------------------------------------------------------------

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.localClientID);
            _packet.Write(NetworkUI.instance.usernameInputField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(Vector3 _position, Quaternion _rotation) 
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_position);
            _packet.Write(_rotation);
            SendUDPData(_packet);
        }
    }

    public static void ChatMessage()
    {
        string message = NetworkUI.instance.chatInputField.text;
        using (Packet _packet = new Packet((int)ClientPackets.chatMessage))
        {
            _packet.Write(Client.instance.localClientID);
            _packet.Write(message);
            SendTCPData(_packet);
        }

        if (message.ToLower() == "quit") 
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    public static void SubmitScore(int raceID, float time) 
    {
        using (Packet _packet = new Packet((int)ClientPackets.submitScore))
        {
            _packet.Write(NetworkedGameManager.players[Client.instance.localClientID].username);
            _packet.Write(raceID);
            _packet.Write(time);
        }
    }
}
