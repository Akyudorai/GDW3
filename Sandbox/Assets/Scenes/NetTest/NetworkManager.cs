using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    // VARIABLES & COMPONENTS
    // ----------------------------------------------------------------------------

    public static NetworkManager instance;
    public static Dictionary<int, NetworkIdentity> players = new Dictionary<int, NetworkIdentity>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public bool debug_isConnected = false;

    // ----------------------------------------------------------------------------
    // MONOBEHAVIOR
    // ----------------------------------------------------------------------------

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Update()
    {
        debug_isConnected = Client.isConnected;
    }

    // ----------------------------------------------------------------------------
    // GAME MANAGEMENT
    // ----------------------------------------------------------------------------

    public void SpawnPlayer(int clientID, string _username, Vector3 _position, Quaternion _rotation)
    {
        NetworkIdentity _player = new NetworkIdentity();
        _player.localClientID = clientID;
        _player.username = _username;

        // TODO: ONLY SPAWN OBJECT IF IN A NETWORKED SCENE
        //if (clientID == Client.instance.localClientID)
        //{
        //    _player = Instantiate(localPlayerPrefab, _position, _rotation);
        //}
        //else
        //{
        //    _player = Instantiate(playerPrefab, _position, _rotation);
        //}

        //_player.GetComponent<NetworkIdentity>().localClientID = clientID;
        //_player.GetComponent<NetworkIdentity>().username = _username;
        players.Add(clientID, _player);

        ClientSend.SubmitScore(0, 100.0f);
    }
}
