using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ConditionalCollectable : Collectable
{
    public UnityEvent onTriggerSuccess;
    public UnityEvent onTriggerFailure;

    public virtual void OnTriggerSuccess(PlayerInteraction playerInteraction)
    {
        onTriggerSuccess.Invoke();
    }
    public virtual void OnTriggerFailure(PlayerInteraction playerInteraction)
    {
        onTriggerFailure.Invoke();
    }

    public abstract bool ConditionalCheck(PlayerInteraction playerInteraction);

    public override void BeginTrigger(PlayerInteraction playerInteraction)
    {
        base.BeginTrigger(playerInteraction);

        if (ConditionalCheck(playerInteraction))
        {
            OnTriggerSuccess(playerInteraction);
        } 
        else
        {
            OnTriggerFailure(playerInteraction);
        }
    }

}
