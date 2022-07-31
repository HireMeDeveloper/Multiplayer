using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BonusPoints : MagicDropPowerup
{

    private void Awake()
    {
        duration = 1f;
    }
    [PunRPC]
    public override void endRPC()
    {
        
    }

    [PunRPC]
    public override void startRPC()
    {
        var data = playerInteraction.getPlayerData();
        if(data != null) data.addPoints(1000);
    }
}
