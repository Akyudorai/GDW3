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
            _packet.Write(PlayerIdentity.Username);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(Vector3 _position, Quaternion _rotation) 
    {       
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_position);
            _packet.Write(_rotation);

            /// Server doesnt seem to be receiving UDP for some reason
            SendUDPData(_packet);
            //SendTCPData(_packet);
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
            _packet.Write(NetworkManager.players[Client.instance.localClientID].username);
            _packet.Write(raceID);
            _packet.Write(time);
            SendTCPData(_packet);
        }
    }

    public static void JoinMultiplayer(int _clientID)
    {
        using (Packet _packet = new Packet((int)ClientPackets.joinMultiplayer))
        {
            _packet.Write(_clientID);
            _packet.Write(PlayerIdentity.Username);
            _packet.Write(PlayerIdentity.Settings.Character);
            SendTCPData(_packet);
        }
    }

    public static void SendAnimationState(int _clientID, AnimationState _state)
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendAnimationState))
        {
            _packet.Write(_clientID);
            _packet.Write(_state.Movement);
            _packet.Write(_state.IsGrounded);
            _packet.Write(_state.SplineControl);
            _packet.Write(_state.IsWallRunning);
            _packet.Write(_state.IsWallRunningRight);
            _packet.Write(_state.IsRailGrinding);
            _packet.Write(_state.IsZiplining);
            _packet.Write(_state.IsLedgeGrabbing);
            SendTCPData(_packet);
        }
    }
}
