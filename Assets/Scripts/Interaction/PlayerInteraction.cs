using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerEvents playerEvents;

    public KeyCode interactButton = KeyCode.F;

    public GameObject textObject;
    public Text textRef;
    public Slider sliderRef;

    private PlayerData data;
    private PlayerStatus playerStatus;
    private ItemUser itemUser;

    private Stack<Collectable> collectables = new Stack<Collectable>();

    private void Awake()
    {
        data = GetComponent<PlayerData>();
        itemUser = GetComponent<ItemUser>();
        playerStatus = GetComponent<PlayerStatus>();
        playerEvents = GetComponent<PlayerEvents>();
    }

    public void setup(Text textRef, Slider sliderRef, ResourceDisplay resourceDisplay)
    {
        this.textObject = textRef.transform.parent.gameObject;
        this.textRef = textRef;
        this.sliderRef = sliderRef;
        data.setResourceDisplay(resourceDisplay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonView.Get(this).IsMine) return;
        var collisionObject = collision.gameObject;
        var enterable = collisionObject.GetComponent<IInteractionEnterable>();

        if (enterable != null) enterable.Enter(this);

        var collectable = collisionObject.GetComponent<Collectable>();

        if (collectable == null) return;

        collectables.Push(collectable);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PhotonView.Get(this).IsMine) return;
        var collisionObject = collision.gameObject;
        var exitable = collisionObject.GetComponent<IInteractionExitable>();

        if (exitable != null) exitable.Exit(this);

        var collectable = collisionObject.GetComponent<Collectable>();

        if (collectable == null) return;

        if(collectables.Peek() == collectable) collectables.Pop();
    }

    private void pressInteract()
    {
        if (!PhotonView.Get(this).IsMine) return;

        if (collectables.Count == 0) return;

        var collectable = collectables.Peek();
        collectable.BeginTrigger(this);
    }
    private void releaseInteract()
    {
        if (!PhotonView.Get(this).IsMine) return;

        if (collectables.Count == 0) return;

        var collectable = collectables.Peek();
        if (collectable is IInteractableEndTriggerable endTriggerable) endTriggerable.EndTrigger(this);
    }

    public ItemUser getItemUser()
    {
        return this.itemUser;
    }

    public PlayerStatus getPlayerStatus()
    {
        return playerStatus;
    }

    public PlayerData getPlayerData()
    {
        return data;
    }

    public bool canPurchase(int cost)
    {
        return cost <= data.getPoints();
    }

    public void purchase(int cost)
    {
        data.removePoints(cost);
        playerEvents?.onPurchase.Invoke();
    }

    private void Update()
    {
        if (!PhotonView.Get(this).IsMine) return;

        if (Input.GetKeyDown(interactButton)) pressInteract();
        if (Input.GetKeyUp(interactButton)) releaseInteract();
    }

    public PlayerEvents getPlayerEvents()
    {
        return this.playerEvents;
    }

    public Collidable PeekAtTriggerable()
    {
        return collectables.Peek();
    }

    public Collidable PopOffTriggerable()
    {
        return collectables.Pop();
    }
}
