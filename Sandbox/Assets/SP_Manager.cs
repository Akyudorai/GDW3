using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_Manager : MonoBehaviour
{
    public GameObject NyxPrefab, BeaPrefab;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        int prefabID = PlayerIdentity.Settings.Character;
        GameObject playerOBJ = null;

        Vector3 _position = SpawnPointManager.GetInstance().SpawnPoints[0].position;
        Quaternion _quaternion = SpawnPointManager.GetInstance().SpawnPoints[0].rotation;

        if (prefabID == 1)
        {
            playerOBJ = Instantiate(NyxPrefab, _position, _quaternion);
        }

        if (prefabID == 2)
        {
            playerOBJ = Instantiate(BeaPrefab, _position, _quaternion);
        }

        if (playerOBJ == null)
        {
            Debug.LogError("Failed to instantiate player prefab");
        }
    }
}
