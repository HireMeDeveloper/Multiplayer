using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerPanel : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public Image background;

    public GameObject leftArrow;
    public GameObject rightArrow;

    public Color personalColor;

    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;

    public void SetPlayerInfo(Player player)
    {
        playerName.text = player.NickName;
        this.player = player;
        updatePlayerPanel(player);
    }

    public void applyLocalChanges()
    {
        background.color = personalColor;
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
    }

    public void onClickLeftArrow()
    {
        var manager = PropertiesManager.instance;
        var avatarValue = (int)player.CustomProperties["playerAvatar"];

        if (avatarValue == 0)
        {
            manager.setPlayerProperty(player, "playerAvatar", avatars.Length - 1);
        }
        else
        {
            manager.setPlayerProperty(player, "playerAvatar", avatarValue - 1);
        }
    }

    public void onClickRightArrow()
    {
        var manager = PropertiesManager.instance;
        var avatarValue = (int)player.CustomProperties["playerAvatar"];

        if (avatarValue == avatars.Length - 1)
        {
            manager.setPlayerProperty(player, "playerAvatar", 0);
        }
        else
        {
            manager.setPlayerProperty(player, "playerAvatar", avatarValue + 1);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(targetPlayer == this.player)
        {
            updatePlayerPanel(targetPlayer);
        }
    }

    private void updatePlayerPanel(Player player)
    {
        var manager = PropertiesManager.instance;
        if (!player.CustomProperties.ContainsKey("playerAvatar"))
        {
            manager.setPlayerProperty(player, "playerAvatar", tryGetUniqueAvatar());
        }

        playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
    }

    private int tryGetUniqueAvatar()
    {
        HashSet<int> avatars = new HashSet<int>();
        
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if(player.Value.CustomProperties.ContainsKey("playerAvatar")) avatars.Add((int)player.Value.CustomProperties["playerAvatar"]);
        }

        for (int i = 0; i < this.avatars.Length; i++)
        {
            if (!avatars.Contains(i))
            {
                return i;
            }
        }

        return 0;
    }
}
