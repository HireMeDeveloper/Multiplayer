using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowRepair : RepeatingAction, IDynamicTextCreator
{
    public Window window;

    public string GetTextFromCreator()
    {
        return "Hold F to Repair Window";
    }

    protected override void OnActionRepeat(PlayerInteraction playerInteraction)
    {
        if (window.RepairBoard()) playerInteraction.getPlayerData().addPoints(10);
    }
}
