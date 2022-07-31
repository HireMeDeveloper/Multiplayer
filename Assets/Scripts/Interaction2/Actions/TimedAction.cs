using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TimedAction : Action
{
    [SerializeField] private float duration;
    private float remainingDuration;

    public UnityEvent onActionUpdate;
    public UnityEvent onActionComplete;

    private IEnumerator ActionTimer(PlayerInteraction playerInteraction)
    {
        remainingDuration = duration;

        while (remainingDuration > 0)
        {
            yield return new WaitForEndOfFrame();

            remainingDuration -= Time.deltaTime;
            onActionUpdate.Invoke();
        }
        onActionComplete.Invoke();
        OnActionComplete(playerInteraction);
    }

    protected abstract void OnActionComplete(PlayerInteraction playerInteraction);

    protected override void StartAction(PlayerInteraction playerInteraction)
    {
        StartCoroutine("ActionTimer", playerInteraction);
    }

    protected override void StopAction(PlayerInteraction playerInteraction)
    {
        StopAllCoroutines();
    }

    public float GetActionPercent()
    {
        return 1.0f - (remainingDuration / duration);
    }

}
