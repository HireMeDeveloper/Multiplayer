using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideCollidable : Collidable
{
    [SerializeField] private GameObject showHideObject;
    public override void Enter(PlayerInteraction playerInteraction)
    {
        base.Enter(playerInteraction);

        showHideObject.SetActive(false);
    }

    public override void Exit(PlayerInteraction playerInteraction)
    {
        base.Exit(playerInteraction);

        showHideObject.SetActive(true);
    }
}
