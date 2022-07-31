using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enterable : MonoBehaviour, IInteractionEnterable
{
    public UnityEvent onEnter;
    public virtual void Enter(PlayerInteraction playerInteraction)
    {
        onEnter.Invoke();
    }
}
