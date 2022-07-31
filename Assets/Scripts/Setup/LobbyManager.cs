using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Text roomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 1.5f;
    private float nextUpdateTime;

    private List<PlayerPanel> playerPanelList = new List<PlayerPanel>();
    public PlayerPanel playerPanelPrefab;
    public Transform playerPanelParent;

    public Button playButton;
    public GameObject joinInProgressButton;
    public Text joinInProgressText;

    public GameObject playerNotification;
    private float playerNotificationTime = 1f;

    [SerializeField] private SettingsData settingsData;

    private void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.CreateRoom("Single Player", new RoomOptions() { MaxPlayers = 1, BroadcastPropsChangeToAll = true });
            return;
        }

        PhotonNetwork.JoinLobby();
    }


    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playButton.gameObject.SetActive(true);
            joinInProgressButton.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(false);
            joinInProgressButton.SetActive(false);
        }
    }

    public void onPressCreate()
    {
        if (roomInputField.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 3, BroadcastPropsChangeToAll = true });
        }
    }

    public override void OnJoinedRoom()
    {
        settupRoom();
        StartCoroutine("notificationTimer");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    private IEnumerator notificationTimer()
    {
        while (PhotonNetwork.CurrentRoom != null)
        {
            if (charactersAreDifferent())
            {
                playerNotification.SetActive(false);
                if (PhotonNetwork.IsMasterClient) playButton.interactable = true;
            }
            else
            {
                playerNotification.SetActive(true);
                if (PhotonNetwork.IsMasterClient) playButton.interactable = false;
            }

            yield return new WaitForSeconds(playerNotificationTime);
        }
    }

    private bool charactersAreDifferent()
    {
        HashSet<int> avatarSet = new HashSet<int>();

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            avatarSet.Add((int)player.Value.CustomProperties["playerAvatar"]);
        }

        return avatarSet.Count >= PhotonNetwork.CurrentRoom.PlayerCount;
    }

    private void settupRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        updatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        if(Time.time >= nextUpdateTime)
        {
            updateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    private void updateRoomList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach(RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.setRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    private void updatePlayerList()
    {
        foreach(PlayerPanel panel in playerPanelList)
        {
            Destroy(panel.gameObject);
        }
        playerPanelList.Clear();

        if (PhotonNetwork.CurrentRoom == null) return;
        var index = 0;

        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerPanel playerPanel = Instantiate(playerPanelPrefab, playerPanelParent);
            playerPanel.SetPlayerInfo(player.Value);
            if (player.Value == PhotonNetwork.LocalPlayer) playerPanel.applyLocalChanges();
            playerPanelList.Add(playerPanel);
            index++;
        }
    }

    public void onClickLeaveRoom()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.Disconnect();
            return;
        }

        StopCoroutine("notificationTimer");
        PhotonNetwork.LeaveRoom();
    }

    public void onClickReturnToMenu()
    {
        PhotonNetwork.Disconnect(); 
    }

    public void onClickPlay()
    {
        if (!charactersAreDifferent()) return;

        PhotonNetwork.CurrentRoom.IsVisible = (bool)PhotonNetwork.CurrentRoom.CustomProperties["joinInProgress"];
        PhotonNetwork.LoadLevel("Game2");
    }

    public void onClickTogglePrivate()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
        if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["joinInProgress"])
        {
            roomProperties["joinInProgress"] = false;
            joinInProgressText.text = "JoinInProgress: disabled";
        }
        else
        {
            roomProperties["joinInProgress"] = true;
            joinInProgressText.text = "JoinInProgress: enabled";
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnLeftRoom()
    {
        if (PhotonNetwork.OfflineMode) return;

        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        updatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        updatePlayerList();
    }

    public override void OnCreatedRoom()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties["joinInProgress"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene("Menu");
    }
}
