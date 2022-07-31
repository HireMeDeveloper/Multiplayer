using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PingDisplay : MonoBehaviour
{
    public Text textComponent;

    private void Update()
    {
        textComponent.text = PhotonNetwork.GetPing().ToString();
    }
}
