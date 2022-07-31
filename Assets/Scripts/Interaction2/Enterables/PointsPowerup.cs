using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPowerup : Enterable
{
    [SerializeField] private int pointsAmount = 40;

    public override void Enter(PlayerInteraction playerInteraction)
    {
        base.Enter(playerInteraction);

        var data = playerInteraction.getPlayerData();

        data.addPoints(pointsAmount);
        Destroy(this.gameObject);
    }
}
