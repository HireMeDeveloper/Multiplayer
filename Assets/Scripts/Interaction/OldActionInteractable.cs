using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class OldActionInteractable : MonoBehaviour
{

    public string canInteractMessage;
    public string cantInteractMessage;

    public float actionDuration = 5f;

    public bool saveProgress = false;

    private float remainingHoldTime;

    private void Start()
    {
        remainingHoldTime = actionDuration;
    }

    public abstract void interact(PlayerInteraction playerInteraction);

    public virtual bool canInteract(PlayerInteraction playerInteraction)
    {
        return true;
    }

    public void onEnter(PlayerInteraction playerInteraction)
    {
        playerInteraction.textRef.text = (canInteract(playerInteraction)) ? canInteractMessage : cantInteractMessage;
        playerInteraction.textObject.SetActive(true);
    }

    public void onExit(PlayerInteraction playerInteraction)
    {
        StopAllCoroutines();
        playerInteraction.textObject.SetActive(false);
        playerInteraction.sliderRef.gameObject.SetActive(false);
    }

    public void onPressInteract(PlayerInteraction playerInteraction)
    {
        if (!canInteract(playerInteraction)) return;

        playerInteraction.sliderRef.gameObject.SetActive(true);
        StartCoroutine("holdTimer", playerInteraction);
    }

    public void onReleaseInteract(PlayerInteraction playerInteraction) 
    {
        StopAllCoroutines();
        playerInteraction.sliderRef.gameObject.SetActive(false);
    }

    private void finishAction(PlayerInteraction playerInteraction)
    {
        playerInteraction.sliderRef.gameObject.SetActive(false);
        playerInteraction.textObject.SetActive(false);
        interact(playerInteraction);
    }

    private IEnumerator holdTimer(PlayerInteraction playerInteraction)
    {
        if(!saveProgress) remainingHoldTime = actionDuration;
        while (remainingHoldTime > 0)
        {
            yield return new WaitForEndOfFrame();
            remainingHoldTime -= Time.deltaTime;

            var percent = 1 - (remainingHoldTime / actionDuration);
            playerInteraction.sliderRef.value = percent;
        }
        finishAction(playerInteraction);
    }
}
