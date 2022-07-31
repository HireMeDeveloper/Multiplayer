using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerup : Enterable
{
    [SerializeField] private float ammoPercent = 10f;

    public override void Enter(PlayerInteraction playerInteraction)
    {
        base.Enter(playerInteraction);

        var user = playerInteraction.getItemUser();

        user.addAmmo(ammoPercent);
        Destroy(this.gameObject);
    }

}
