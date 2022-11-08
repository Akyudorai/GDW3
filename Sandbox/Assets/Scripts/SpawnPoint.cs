using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos() // This is a fun little trick to make the NPC spawnpoints visible in the editor but not ingame :)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, Vector3.one);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
