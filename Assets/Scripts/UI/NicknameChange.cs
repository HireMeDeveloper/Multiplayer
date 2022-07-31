using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NicknameChange : MonoBehaviour
{
    public Text currentName;
    public InputField usernameInput;

    private void Start()
    {
        updateName();
    }
    public void onChangeName()
    {
        if (usernameInput.text.Length > 0)
        {
            PhotonNetwork.NickName = usernameInput.text;
            updateName();
        }
    }

    private void updateName()
    {
        currentName.text = PhotonNetwork.NickName;
    }
}
