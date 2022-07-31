using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TriplePoints : MagicDropPowerup
{
    [PunRPC]
    public override void endRPC()
    {
        var multiplier = playerInteraction.getPlayerData().pointsGainMultiplier;
        playerInteraction.getPlayerData().setPointsGainMultiplierOverNetwork(multiplier / 3);
    }

    [PunRPC]
    public override void startRPC()
    {
        var multiplier = playerInteraction.getPlayerData().pointsGainMultiplier;
        playerInteraction.getPlayerData().setPointsGainMultiplierOverNetwork(multiplier * 3);
    }
}
