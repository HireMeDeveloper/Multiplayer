using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Enterable
{
    [SerializeField] private int healAmount = 25;

    public override void Enter(PlayerInteraction playerInteraction)
    {
        base.Enter(playerInteraction);

        var status = playerInteraction.getPlayerStatus();

        status.heal(healAmount);
        Destroy(this.gameObject);
    }
}
