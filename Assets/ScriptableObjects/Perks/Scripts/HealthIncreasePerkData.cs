using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PerkData/HealthIncreasePerkData")]
public class HealthIncreasePerkData : PerkData
{
    [SerializeField] private int increaseAmount = 50;

    public override void OnLose(PlayerInteraction playerInteraction)
    {
        var status = playerInteraction.getPlayerStatus();
        var data = playerInteraction.getPlayerData();

        status.resetMaxHealth();

        data.clearPerks();
    }

    public override void OnPurchase(PlayerInteraction playerInteraction)
    {
        var status = playerInteraction.getPlayerStatus();
        var data = playerInteraction.getPlayerData();

        status.increaseMaxHealthBy(increaseAmount);
        data.addPerk(this);
    }

    public override void OnRePurchase(PlayerInteraction playerInteraction)
    {
        
    }
}
