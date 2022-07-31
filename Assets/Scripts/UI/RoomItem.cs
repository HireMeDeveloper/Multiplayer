using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class RoomItem : MonoBehaviour
{
    public Button roomButton;
    public Text roomName;

    public void setRoomName(string roomName)
    {
        this.roomName.text = roomName;
        setButtonEvent(roomName);
    }

    public void setButtonEvent(string roomName)
    {
        roomButton.onClick.AddListener(() => PhotonNetwork.JoinRoom(roomName));
    }
}
