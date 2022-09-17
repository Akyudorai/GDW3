using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    //public NetworkRunner networkRunnerPrefab;
    NetworkRunner runner;

    public NetworkPrefabRef playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private void Start() 
    {
        DontDestroyOnLoad(this);
    }

    async void StartGame(GameMode mode) 
    {
        // Create the Fusion runner and let it know that we will be providing input
        runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await runner.StartGame(new StartGameArgs() 
        {
            GameMode = mode,
            SessionName = "Test Room",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        if (runner.IsServer) 
        {
            Debug.Log("OnPlayerJoined we are server.  Spawning Player");
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
            spawnedCharacters.Add(player, networkPlayerObject);
        }
        else Debug.Log("OnPlayerJoined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    { 
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject)) 
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W)) data.direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) data.direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) data.direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) data.direction += Vector3.right;

        input.Set(data);
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

    private void OnGUI() 
    {
        if (runner == null) 
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }

            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}
