using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/PerkData/ReloadSpeedPerkData")]

public class ReloadSpeedPerkData : PerkData
{
    [SerializeField] private float reloadSpeedModifier = 0.6f;
    public override void OnLose(PlayerInteraction playerInteraction)
    {
        var data = playerInteraction.getPlayerData();

        data.reloadSpeedMultiplier /= reloadSpeedModifier;

        data.clearPerks();
    }

    public override void OnPurchase(PlayerInteraction playerInteraction)
    {
        var data = playerInteraction.getPlayerData();

        data.reloadSpeedMultiplier *= reloadSpeedModifier;
        data.addPerk(this);
    }

    public override void OnRePurchase(PlayerInteraction playerInteraction)
    {
        
    }
}
