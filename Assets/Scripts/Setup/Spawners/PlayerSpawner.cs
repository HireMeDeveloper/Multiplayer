using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerSpawner : MonoBehaviourPun
{
    public static PlayerSpawner instance;

    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    public CameraController followLocalPlayer;

    public Text textRef;
    public Slider sliderRef;
    public ResourceDisplay resourceDisplay;

    public UnityEvent onSpawnPlayers;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {

        GameObject playerToSpawn = playerPrefabs[(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("playerAvatar")) ? (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] : 0];
        var player = PhotonNetwork.Instantiate(playerToSpawn.name, getRandomSpawnPosition(), Quaternion.identity);

        var locPlayer = PhotonNetwork.LocalPlayer;
        var playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null) playerInteraction.setup(textRef, sliderRef, resourceDisplay);

        var view = player.GetPhotonView();

        followLocalPlayer.connectCamera(player);

        //onSpawnPlayers.Invoke();
        var raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = ReceiverGroup.All;
        PhotonNetwork.RaiseEvent(EventCodes.OnPlayerSpawnEvent, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public Vector2 getRandomSpawnPosition()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        return spawnPoints[rand].position;
    }
}
