using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;
using Fusion.Sockets;
using System;

public class NetworkedSpawnManager : MonoBehaviour, INetworkRunnerCallbacks
{
    //public NetworkPrefabRef playerPrefab;
    //private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        // if (runner.IsServer) 
        // {
        //     Debug.Log("OnPlayerJoined we are server.  Spawning Player");
        //     NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
        //     spawnedCharacters.Add(player, networkPlayerObject);
        // }
        // else Debug.Log("OnPlayerJoined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    { 
        // if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject)) 
        // {
        //     runner.Despawn(networkObject);
        //     spawnedCharacters.Remove(player);
        // }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        // var data = new NetworkInputData();

        // if (Input.GetKey(KeyCode.W)) data.direction += Vector3.forward;
        // if (Input.GetKey(KeyCode.S)) data.direction += Vector3.back;
        // if (Input.GetKey(KeyCode.A)) data.direction += Vector3.left;
        // if (Input.GetKey(KeyCode.D)) data.direction += Vector3.right;

        // input.Set(data);
    }
   
    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer"); }
    
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Debug.Log("OnShutdown"); }
    public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDisconnectedFromServer"); }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest"); }
    public void OnConnectFailed(NetworkRunner runner, NetAddress removeAdress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed"); }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { } 
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }


}
