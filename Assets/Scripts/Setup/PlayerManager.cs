using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using ExitGames.Client.Photon;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public UnityEvent onAllPlayersDead;

    private bool foundAllPlayersDead = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    private void Update()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        if (!foundAllPlayersDead && players.Length == 0)
        {
            foundAllPlayersDead = true;
            onAllPlayersDead.Invoke();
            sendAllPlayersToLobby();
        }
    }

    public void sendAllPlayersToLobby()
    {
        PhotonNetwork.LoadLevel("ConnectToServer");
    }

    public void respawnAllPlayers()
    {
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            var view = PhotonView.Get((GameObject)player.Value.TagObject);
            var status = view.GetComponent<PlayerStatus>();
            if (status != null) status.tryRespawn();
        }
    }

}
