using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PerkData : ScriptableObject
{
    public string perkName;
    public Sprite perkIcon;
    public Sprite proPerkIcon;
    public string perkDescription;
    public string proPerkDescription = "None";

    public abstract void OnPurchase(PlayerInteraction playerInteraction);
    public abstract void OnRePurchase(PlayerInteraction playerInteraction);
    public abstract void OnLose(PlayerInteraction playerInteraction);
}
