using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatus : Damageable
{
    private PlayerEvents playerEvents;

    private int currentHealth;
    private int maxHealth;
    private const int DEFAULT_HEALTH = 100;

    private int regenAmount = 1;
    private float regenDelay = 5f;
    private const float DEFAULT_REGEN_DELAY = 5f;
    private float regenRate = 0.15f;
    private const float DEFAULT_REGEN_RATE = 0.15f;

    private float baseSpeed = 6f;
    public float speedMultiplier { get; set; } = 1;

    [SerializeField] private float invulnerableDuration;
    private bool isInvulnerable = false;

    private PhotonView view;

    private PlayerData data;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        data = GetComponent<PlayerData>();
        playerEvents = GetComponent<PlayerEvents>();
    }

    private void Start()
    {
        resetMaxHealth();
    }

    public void tryRespawn()
    {
        
    }

    private void respawnPlayer()
    {
        currentHealth = maxHealth;
        data.getResourceDisplay().updateHealthBar((float)currentHealth / maxHealth);
        gameObject.transform.position = PlayerSpawner.instance.getRandomSpawnPosition();
        gameObject.tag = "Player";
        setShown(true);
    }

    public override void damage(int damage, Player damager)
    {
        if (isInvulnerable) return;

        playerEvents?.onHit.Invoke();
        base.damage(damage, damager);

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        ResourceDisplay resourceDisplay = null;
        if (data != null) resourceDisplay = data.getResourceDisplay();
        if(resourceDisplay != null) resourceDisplay.updateHealthBar((float)currentHealth / maxHealth);
        if (currentHealth == 0) death();

        setInvulnerable();
        Invoke("setVulnerable", invulnerableDuration);

        if (!gameObject.activeInHierarchy) return;
        StopCoroutine("healTimer");
        StartCoroutine("healTimer");
    }

    public override void death()
    {
        base.death();

        gameObject.tag = "DeadPlayer";
        data.isAlive = false;
        setShown(false);
    }

    public void heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        data.getResourceDisplay().updateHealthBar((float)currentHealth / maxHealth);
    }

    public IEnumerator healTimer()
    {
        yield return new WaitForSeconds(regenDelay);

        while (currentHealth < maxHealth)
        {
            yield return new WaitForSeconds(regenRate);
            heal(regenAmount);
        }
    }

    public float getSpeed()
    {
        return baseSpeed * speedMultiplier;
    }

    private void setInvulnerable()
    {
        isInvulnerable = true;
    }

    private void setVulnerable()
    {
        isInvulnerable = false;
    }

    public void setShown(bool value)
    {
        view.RPC("setShownRPC", RpcTarget.All, value);
    }

    [PunRPC]
    private void setShownRPC(bool value)
    {
        gameObject.SetActive(value);
        if (!value) Invoke("respawnPlayer", 10f);
    }

    public void increaseMaxHealthBy(int health)
    {
        maxHealth = DEFAULT_HEALTH + health;
        currentHealth = maxHealth;
    }

    public void resetMaxHealth()
    {
        maxHealth = DEFAULT_HEALTH;
        currentHealth = maxHealth;
    }


    public void boostRegenRate(float multiplier)
    {
        regenRate = DEFAULT_REGEN_RATE * multiplier;
        regenDelay = DEFAULT_REGEN_DELAY * multiplier;
    }

    public void resetRegenRate()
    {
        regenRate = DEFAULT_REGEN_RATE;
        regenDelay = DEFAULT_REGEN_DELAY;
    }

    public void startSpeedBoost(PlayerInteraction playerInteraction)
    {
        StopAllCoroutines();
        StartCoroutine("speedBoostTimer", playerInteraction);
    }

    private IEnumerator speedBoostTimer(PlayerInteraction playerInteraction)
    {
        var boostMultiplier = 1.3f;
        var boostDuration = 2f;

        playerInteraction.getPlayerStatus().speedMultiplier *= boostMultiplier;
        yield return new WaitForSeconds(boostDuration);
        playerInteraction.getPlayerStatus().speedMultiplier /= boostMultiplier;
    }
}
