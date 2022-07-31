using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Purchasable : ConditionalCollectable
{

    [SerializeField] protected int price;
    public override bool ConditionalCheck(PlayerInteraction playerInteraction)
    {
        return playerInteraction.canPurchase(price);
    }

}
