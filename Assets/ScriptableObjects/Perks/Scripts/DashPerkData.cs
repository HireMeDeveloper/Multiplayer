using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PerkData/DashPerkData")]
public class DashPerkData : PerkData
{
    public override void OnLose(PlayerInteraction playerInteraction)
    {
        var events = playerInteraction.getPlayerEvents();
        var status = playerInteraction.getPlayerStatus();
        var data = playerInteraction.getPlayerData();

        events.onDodgeRoll.RemoveListener(() =>
            status.startSpeedBoost(playerInteraction)
        );

        data.clearPerks();
    }

    public override void OnPurchase(PlayerInteraction playerInteraction)
    {
        var events = playerInteraction.getPlayerEvents();
        var status = playerInteraction.getPlayerStatus();
        var data = playerInteraction.getPlayerData();

        events.onDodgeRoll.AddListener(() =>
            status.startSpeedBoost(playerInteraction)
        );

        data.addPerk(this);
    }

    public override void OnRePurchase(PlayerInteraction playerInteraction)
    {
        
    }
}
