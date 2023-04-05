using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedGameManager : MonoBehaviour
{
    private void Awake()
    {
        ClientSend.JoinMultiplayer(Client.instance.localClientID);

    }
}
