using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

[RequireComponent(typeof(PhotonView))]
public class DebrisInteractable : Purchasable, IDynamicTextCreator
{
    public string GetTextFromCreator()
    {
        return "Press F to Clear Debris [" + price + "]";
    }

    public override void OnTriggerSuccess(PlayerInteraction playerInteraction)
    {
        base.OnTriggerSuccess(playerInteraction);

        playerInteraction.purchase(price);

        var view = GetComponent<PhotonView>();
        view.RPC("interactRPC", RpcTarget.All, null);
    }

    [PunRPC]
    private void interactRPC()
    {
        this.gameObject.SetActive(false);
    }


}
