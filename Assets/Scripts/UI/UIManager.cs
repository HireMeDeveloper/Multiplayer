using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviour
{
    private int currentWave = 1;
    public Text roundCounter;

    private List<PlayerCard> playerCardList = new List<PlayerCard>();
    public PlayerCard playerCardPrefab;

    public Transform playerCardParent;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        if (obj.Code != EventCodes.OnPlayerSpawnEvent) return;
        setupUi();
    }

    public void setupUi()
    {
        updatePlayerCards();
    }

    public void incrementWave()
    {
        currentWave++;
        roundCounter.text = getRoundValue(currentWave);
    }

    public void updatePoints()
    {
        foreach (PlayerCard card in playerCardList)
        {
            //card.updatePoints();
        }
    }

    private void updatePlayerCards()
    {
        foreach (PlayerCard card in playerCardList)
        {
            Destroy(card.gameObject);
        }
        playerCardList.Clear();

        if (PhotonNetwork.CurrentRoom == null) return;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            var view = PhotonView.Get((GameObject)player.Value.TagObject);

            if (view == null) return;

            var data = view.gameObject.GetComponent<PlayerData>();

            if (data == null) return;

            PlayerCard playerCard = Instantiate(playerCardPrefab, playerCardParent);
            playerCard.SetPlayerInfo(player.Value, data);
            data.playerCard = playerCard;

            if (player.Value == PhotonNetwork.LocalPlayer) playerCard.applyLocalChanges();
            playerCardList.Add(playerCard);
        }
    }

    private string getRoundValue(int round)
    {
        var roundString = round.ToString();
        return (round < 10) ? "000" + roundString : "00" + roundString;
    }
}
