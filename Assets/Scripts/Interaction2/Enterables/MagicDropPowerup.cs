using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public abstract class MagicDropPowerup  : Enterable
{
    protected float duration = 15;
    protected PlayerInteraction playerInteraction;
    public PhotonView view;

    private void Awake()
    {
        playerInteraction = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerInteraction>();
    }

    public override void Enter(PlayerInteraction playerInteraction)
    {
        base.Enter(playerInteraction);

        view.RPC("hide", RpcTarget.All);
        view.RPC("startRPC", RpcTarget.All);

        StartCoroutine("powerupTimer");
    }

    private IEnumerator powerupTimer()
    {
        yield return new WaitForSeconds(duration);
        endInteract();
    }
    private void endInteract()
    {
        view.RPC("endRPC", RpcTarget.All);
        view.RPC("remove", RpcTarget.All);
    }

    public abstract void startRPC();

    public abstract void endRPC();

    [PunRPC]
    public void hide()
    {
        var collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;

        var anim = GetComponent<Animator>();

        anim.SetTrigger("Activate");
    }

    [PunRPC]
    public void remove()
    {
        Destroy(this.gameObject);
    }

}
