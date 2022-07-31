using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InfinateAmmo : MagicDropPowerup
{
    [PunRPC]
    public override void endRPC()
    {
        playerInteraction.getItemUser().losesAmmo = true;
    }

    [PunRPC]
    public override void startRPC()
    {
        var user = playerInteraction.getItemUser();
        user.addAmmo(1);
        user.losesAmmo = false;
    }
}
