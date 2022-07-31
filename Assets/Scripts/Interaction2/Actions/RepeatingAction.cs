using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RepeatingAction : Action
{
    [SerializeField] private float delay;
    private bool isRepeating = false;

    public UnityEvent onActionRepeat;

    private IEnumerator ActionTimer(PlayerInteraction playerInteraction)
    {
        while (isRepeating)
        {
            yield return new WaitForSeconds(delay);
            onActionRepeat.Invoke();
            OnActionRepeat(playerInteraction);
        }
    }

    protected abstract void OnActionRepeat(PlayerInteraction playerInteraction);

    protected override void StartAction(PlayerInteraction playerInteraction)
    {
        isRepeating = true;
        StartCoroutine("ActionTimer", playerInteraction);
    }

    protected override void StopAction(PlayerInteraction playerInteraction)
    {
        StopAllCoroutines();
        isRepeating = false;
    }
}
