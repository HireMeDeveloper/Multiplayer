using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : Purchasable, IDynamicTextCreator
{
    [SerializeField] private PerkData perkData;
    protected bool isPowered = false;

    public void powerOn()
    {
        isPowered = true;
    }

    public override bool ConditionalCheck(PlayerInteraction playerInteraction)
    {
        return base.ConditionalCheck(playerInteraction) && isPowered;
    }

    public override void OnTriggerSuccess(PlayerInteraction playerInteraction)
    {
        base.OnTriggerSuccess(playerInteraction);

        playerInteraction.purchase(price);
        perkData.OnPurchase(playerInteraction);
    }

    public string GetTextFromCreator()
    {
        return (isPowered) ? "Press F to Buy " + perkData.name + " [" + price + "]" : "Need more soul Power!";
    }
}
