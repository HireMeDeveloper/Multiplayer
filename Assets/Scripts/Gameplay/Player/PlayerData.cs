using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class PlayerData : MonoBehaviour, IPunObservable
{

    [SerializeField] private int points = 500;
    public float pointsGainMultiplier { get; private set; } = 1.0f;
    private float pointsDiscountMultiplier = 1.0f;

    public bool isAlive { get; set; } = true;

    public float damageMultiplier { get; private set; } = 1f;
    public float reloadSpeedMultiplier { get; set; } = 1.0f;

    public PlayerCard playerCard;

    private PhotonView view;

    private ResourceDisplay resourceDisplay;
    private UiRefrenceManager refrenceManager;

    public UnityEvent onKill;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        var uiManager = GameObject.FindGameObjectWithTag("UiManager");
        if (uiManager == null) return;

        refrenceManager = uiManager.GetComponent<UiRefrenceManager>();
    }

    public void addPerk(PerkData perkData)
    {
        refrenceManager.addPerkIcon(perkData);
    }

    public void clearPerks()
    {
        refrenceManager.clearPerkIcons();
    }

    public void addPoints(int points)
    {
        this.points += Mathf.FloorToInt(points * pointsGainMultiplier);
        view.RPC("updatePointsRPC", RpcTarget.All, this.points);
    }

    public void grantKill()
    {
        onKill.Invoke();
    }

    public void removePoints(int points)
    {
        this.points -= Mathf.FloorToInt(points * pointsDiscountMultiplier);
        view.RPC("updatePointsRPC", RpcTarget.All, this.points);
    }

    [PunRPC]
    private void updatePointsRPC(int points)
    {
        this.points = points;
        if (playerCard == null) return;
        playerCard.updatePoints(points);
    }

    public void setResourceDisplay(ResourceDisplay resourceDisplay)
    {
        this.resourceDisplay = resourceDisplay;
    }
    public ResourceDisplay getResourceDisplay()
    {
        return this.resourceDisplay;
    }
    public int getPoints()
    {
        return this.points;
    }

    public void setDamageMultiplierOverNetwork(float multiplier)
    {
        view.RPC("setDamageMultiplier", RpcTarget.All, multiplier);
    }

    [PunRPC]
    public void setDamageMultiplier(float multiplier)
    {
        this.damageMultiplier = multiplier;
    }

    public void setPointsGainMultiplierOverNetwork(float multiplier)
    {
        view.RPC("setPointsGainMultiplier", RpcTarget.All, multiplier);
    }

    [PunRPC]
    public void setPointsGainMultiplier(float multiplier)
    {
        this.pointsGainMultiplier = multiplier;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
