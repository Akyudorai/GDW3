using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedGameManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    // VARIABLES & COMPONENTS
    // ----------------------------------------------------------------------------

    public static NetworkedGameManager instance;
    public static Dictionary<int, NetworkIdentity> players = new Dictionary<int, NetworkIdentity>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    // ----------------------------------------------------------------------------
    // MONOBEHAVIOR
    // ----------------------------------------------------------------------------

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    // ----------------------------------------------------------------------------
    // GAME MANAGEMENT
    // ----------------------------------------------------------------------------

    public void SpawnPlayer(int clientID, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;

        if (clientID == Client.instance.localClientID) {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<NetworkIdentity>().localClientID = clientID;
        _player.GetComponent<NetworkIdentity>().username = _username;
        players.Add(clientID, _player.GetComponent<NetworkIdentity>());
    }

}
