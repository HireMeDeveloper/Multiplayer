using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCard : MonoBehaviour
{
    public Image playerHead;
    public GameObject background;
    public Text playerPoints;

    public Color personalPointsColor;

    [SerializeField] private List<Sprite> playerHeads = new List<Sprite>();

    private Player player;
    private PlayerData data;

    public void SetPlayerInfo(Player player, PlayerData data)
    {
        var index = (player.CustomProperties.ContainsKey("playerAvatar")) ? (int)player.CustomProperties["playerAvatar"] : 0;
        playerHead.sprite = playerHeads[index];
        this.player = player;
        this.data = data;
        updatePlayerPanel(500);
    }

    public void applyLocalChanges()
    {
        playerPoints.color = personalPointsColor;
        background.SetActive(true);
        playerPoints.fontSize = 50;
    }

    public void updatePoints(int points)
    {
        updatePlayerPanel(points);
    }

    public void updatePlayerPanel(int points)
    {
        playerPoints.text = points.ToString();
    }
}
