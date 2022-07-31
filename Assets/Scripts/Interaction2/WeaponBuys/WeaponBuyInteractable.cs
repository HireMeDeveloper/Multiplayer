using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponBuyInteractable : Purchasable, IDynamicTextCreator
{
    [SerializeField] private Weapon weapon;

    private void Awake()
    {
        base.price = weapon.price;
    }

    public override void OnTriggerSuccess(PlayerInteraction playerInteraction)
    {
        base.OnTriggerSuccess(playerInteraction);

        playerInteraction.purchase(price);
        playerInteraction.getItemUser().equipItem(weapon.weaponType);
        playerInteraction.PopOffTriggerable();
    }

    public string GetTextFromCreator()
    {
        return "Press F To Buy " + weapon.name + " [" + weapon.price + "]";
    }
}
