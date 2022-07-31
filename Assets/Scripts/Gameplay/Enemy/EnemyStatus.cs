using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class EnemyStatus : Damageable
{
    private PhotonView view;
    private int currentHealth;

    [SerializeField] private int pointsPerKill = 90;

    private void Awake()
    {
        this.view = GetComponent<PhotonView>();
    }

    public override void damage(int amount, Player damager)
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;
        view.RPC("damageRPC", RpcTarget.All, null);
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            grantPoints(damager);
            view.RPC("death", RpcTarget.All, null);
            return;
        }
    }

    private void grantPoints(Player player)
    {
        var view = PhotonView.Get((GameObject)player.TagObject);
        if (view == null) return;
        var data = view.GetComponent<PlayerData>();
        if (data == null) return;
        data.addPoints(pointsPerKill);
        data.grantKill();
    }

    public void setHealth(int health)
    {
        currentHealth = health;
    }

    public void heal(int healAmount)
    {
        currentHealth += healAmount;
    }

}
