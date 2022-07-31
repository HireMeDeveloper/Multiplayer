using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Action : Collectable, IInteractableEndTriggerable
{
    public UnityEvent onEndTrigger;

    public override void BeginTrigger(PlayerInteraction playerInteraction)
    {
        base.BeginTrigger(playerInteraction);

        StartAction(playerInteraction);
    }

    public virtual void EndTrigger(PlayerInteraction playerInteraction)
    {
        onEndTrigger.Invoke();

        StopAction(playerInteraction);
    }

    public override void Exit(PlayerInteraction playerInteraction)
    {
        base.Exit(playerInteraction);

        StopAction(playerInteraction);
    }

    protected abstract void StartAction(PlayerInteraction playerInteraction);
    protected abstract void StopAction(PlayerInteraction playerInteraction);


}
