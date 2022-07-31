using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Events;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public InputField usernameInput;
    public Text buttonText;

    [SerializeField] private SettingsData settingsData;
    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();

        

        PhotonNetwork.SendRate = 25;
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!settingsData.isOnline) PhotonNetwork.OfflineMode = true;
        else PhotonNetwork.ConnectUsingSettings();


    }

}
