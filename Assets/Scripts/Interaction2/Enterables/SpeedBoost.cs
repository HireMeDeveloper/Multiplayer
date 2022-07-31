using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpeedBoost : MagicDropPowerup
{
    [PunRPC]
    public override void endRPC()
    {
        playerInteraction.getPlayerStatus().speedMultiplier /= 1.2f;
    }

    [PunRPC]
    public override void startRPC()
    {
        playerInteraction.getPlayerStatus().speedMultiplier *= 1.2f;
    }
}
