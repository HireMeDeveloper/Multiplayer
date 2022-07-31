using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UpgradeInteractable : Purchasable
{
    private int purchases = 0;
    [SerializeField] private int startPrice = 5000;
    [SerializeField] private int pricePerPurchase = 10000;

    private void Start()
    {
        price = startPrice;
        updateMessages();
    }

    private void updateMessages()
    {
        //canInteractMessage = "Press F to Upgrade Sugar-Rush [Cost:" + price + "]";
        //cantInteractMessage = "Upgrade Sugar-Rush [Cost:" + price + "]";
    }

    public override void OnTriggerSuccess(PlayerInteraction playerInteraction)
    {
        base.OnTriggerSuccess(playerInteraction);

        playerInteraction.purchase(price);
        var multiplier = playerInteraction.getPlayerData().damageMultiplier;
        playerInteraction.getPlayerData().setDamageMultiplierOverNetwork(multiplier * 1.5f);
        purchases++;
        price = startPrice + (pricePerPurchase * purchases);
        updateMessages();
    }
}
