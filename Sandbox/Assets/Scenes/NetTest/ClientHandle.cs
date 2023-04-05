using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using UnityEngine.SceneManagement;

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
        int _character = _packet.ReadInt();
        Vector3 _position_packet = _packet.ReadVector3();
        Quaternion _quaternion_packet = _packet.ReadQuaternion();

        Vector3 _position = SpawnPointManager.GetInstance().SpawnPoints[0].position;
        Quaternion _quaternion = SpawnPointManager.GetInstance().SpawnPoints[0].rotation;

        // Only spawn if in a networked scene
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(4))
        {
            NetworkManager.instance.SpawnPlayer(_id, _character, _username, _position, _quaternion);
        }        
    }

    public static void PlayerPosition(Packet _packet) 
    {
        Debug.Log("Receiving UDP Position");

        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        NetworkManager.players[_id].netPC.SetPosition(_position);           
        //NetworkedGameManager.players[_id].transform.position = _position;        
    }

    public static void PlayerRotation(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        NetworkManager.players[_id].netPC.mesh.transform.rotation = _rotation;        
    }

    public static void ChatMessage(Packet _packet) 
    {
        int _id = _packet.ReadInt();
        string _msg = _packet.ReadString();

        Debug.Log($"{NetworkManager.players[_id].username}: {_msg}");

        /// ONLY POST THIS ON MULTIPLAYER SCENE
        //NetworkUI.instance.ReceiveMessage($"{NetworkManager.players[_id].username}: {_msg}");
    }

    public static void PlayerDisconnection(Packet _packet) 
    {
        int _id = _packet.ReadInt();

        //TODO: Handle the destruction of all objects belonging to the ID;
        Debug.Log($"Player with ID [{_id}] has disconnected from the server.");
        Destroy(NetworkManager.players[_id].netPC.gameObject);
        NetworkManager.players.Remove(_id);
    }

    public static void NewHighScore(Packet _packet) 
    {
        string _username = _packet.ReadString();
        int _position = _packet.ReadInt();
        int _raceID = _packet.ReadInt();
        float _time = _packet.ReadFloat();

        // Make an announcement
        string announcement = $"{_username} has claimed #{_position + 1} highscore in {_raceID} with a time of {_time}!";

        Debug.Log(announcement);

        /// ONLY POST THIS ON MULTIPLAYER SCENE
        //NetworkUI.instance.ReceiveMessage($"{_username} has claimed #{_position+1} highscore in {_raceID} with a time of {_time}!");
    }

    public static void JoinMultiplayer(Packet _packet)
    {
        int _clientID = _packet.ReadInt();

        // Do whatever else we need when a player joins the server
    }

    public static void ReceiveAnimationState(Packet _packet)
    {
        int _clientID = _packet.ReadInt();
        float _movement = _packet.ReadFloat();
        bool _isGrounded = _packet.ReadBool();
        bool _splineControlled = _packet.ReadBool();
        bool _isWallRunning = _packet.ReadBool();
        bool _isWallRunningRight = _packet.ReadBool();
        bool _isRailGrinding = _packet.ReadBool();
        bool _isZiplining = _packet.ReadBool();
        bool _isLedgeGrabbing = _packet.ReadBool();

        // Update the Animation State of Specified Character
        NetworkManager.players[_clientID].netPC.animationHandler.SetAnimationState(new AnimationState()
        {
            Movement = _movement,
            IsGrounded = _isGrounded,
            SplineControl = _splineControlled,
            IsWallRunning = _isWallRunning,
            IsWallRunningRight = _isWallRunningRight,
            IsRailGrinding = _isRailGrinding,
            IsZiplining = _isZiplining,
            IsLedgeGrabbing = _isLedgeGrabbing
        });
    }
}