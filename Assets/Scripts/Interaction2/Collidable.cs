using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Collidable : Enterable, IInteractionExitable
{
    public UnityEvent onExit;
    public virtual void Exit(PlayerInteraction playerInteraction) 
    {
        onExit.Invoke();
    }
}
