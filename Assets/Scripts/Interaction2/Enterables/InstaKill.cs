using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InstaKill : MagicDropPowerup
{
    private float amount = 9999;
    [PunRPC]
    public override void endRPC()
    {
        var multiplier = playerInteraction.getPlayerData().damageMultiplier;
        playerInteraction.getPlayerData().setDamageMultiplierOverNetwork(multiplier / amount);
    }

    [PunRPC]
    public override void startRPC()
    {
        var multiplier = playerInteraction.getPlayerData().damageMultiplier;
        playerInteraction.getPlayerData().setDamageMultiplierOverNetwork(multiplier * amount);
    }
}
