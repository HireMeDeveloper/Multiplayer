using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Collectable : Collidable, IInteractionBeginTriggerable
{
    public UnityEvent onBeginTrigger;
    public virtual void BeginTrigger(PlayerInteraction playerInteraction)
    {
        onBeginTrigger.Invoke();
    }
}
