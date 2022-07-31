using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Start()
    {
        var spawnPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        var obj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity);

    }

}
