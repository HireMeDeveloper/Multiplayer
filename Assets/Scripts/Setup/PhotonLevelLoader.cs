using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonLevelLoader : MonoBehaviour
{
    public string sceneName;

    public void loadLevel()
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
