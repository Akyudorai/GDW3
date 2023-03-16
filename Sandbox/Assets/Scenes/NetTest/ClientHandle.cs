using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;

public class ClientHandle : MonoBehaviour
{

    // ----------------------------------------------------------------------------
    // PACKET HANDLER FUNCTIONS
    // ----------------------------------------------------------------------------

    public static void Welcome(Packet _packet) 
    {
        string _msg = _packet.ReadString();
        int incomingID = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.localClientID = incomingID;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _quaternion = _packet.ReadQuaternion();

        NetworkedGameManager.instance.SpawnPlayer(_id, _username, _position, _quaternion);
    }

    public static void PlayerPosition(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        NetworkedGameManager.players[_id].transform.position = _position;        
    }

    public static void PlayerRotation(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        NetworkedGameManager.players[_id].GetComponent<NetworkedPlayerController>().mesh.transform.rotation = _rotation;        
    }

    public static void ChatMessage(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        string _msg = _packet.ReadString();

        NetworkUI.instance.ReceiveMessage($"{NetworkedGameManager.players[_id].username}: {_msg}");
    }
}