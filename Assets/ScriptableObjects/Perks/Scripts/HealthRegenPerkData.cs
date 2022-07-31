using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PerkData/HealthRegenPerkData")]
public class HealthRegenPerkData : PerkData
{
    [SerializeField] private float regenMultiplier = 0.80f;

    public override void OnLose(PlayerInteraction playerInteraction)
    {
        var status = playerInteraction.getPlayerStatus();
        var data = playerInteraction.getPlayerData();

        status.resetRegenRate();

        data.clearPerks();
    }

    public override void OnPurchase(PlayerInteraction playerInteraction)
    {
        var status = playerInteraction.getPlayerStatus();
        var data = playerInteraction.getPlayerData();

        status.boostRegenRate(regenMultiplier);

        data.addPerk(this);
    }

    public override void OnRePurchase(PlayerInteraction playerInteraction)
    {
        
    }
}
