using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkInteractableSpawner : InteractableSpawner
{
    public override void spawn(Transform transform)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.Instantiate(interactable.name, transform.position, Quaternion.identity);
    }
}
