using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class ItemUser : MonoBehaviour
{
    private PhotonView view;
    private PlayerEvents playerEvents;
    private Rigidbody2D rb;
    private PlayerData data;

    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject itemSocket;
    [SerializeField] private bool isEnemy = false;

    [SerializeField] public List<Weapon> guns = new List<Weapon>();

    public Animator itemAnim;
    public UnityEvent onUseLightItem;
    public UnityEvent onUseMediumItem;
    public UnityEvent onUseHeavyItem;

    public bool losesAmmo { get; set; } = true;
    private int currentAmmo;
    private float cooldown;
    private float reloadTime;
    private bool isReloading = false;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        playerEvents = GetComponent<PlayerEvents>();
        rb = GetComponent<Rigidbody2D>();
        data = GetComponent<PlayerData>();
    }

    private void Start()
    {
        currentAmmo = currentWeapon.MaxAmmo;
    }

    private void Update()
    {
        if (!view.IsMine) return;
        //view.RPC("updateItem", RpcTarget.All, null);

        if (isEnemy) return;
        if (cooldown > 0 || reloadTime > 0) return;

        if (Input.GetKey(KeyCode.Mouse0) && currentWeapon.fullAuto) remoteUseItem();
        else if(Input.GetKeyDown(KeyCode.Mouse0)) remoteUseItem();

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < currentWeapon.MaxAmmo) reload();
    }

    public bool canUpgradeItem()
    {
        return false;
    }

    public void upgradeItem()
    {
        
    }

    public void equipItem(WeaponType type)
    {
        view.RPC("equipGun", RpcTarget.All, (int)type);
        view.RPC("updateItem", RpcTarget.All, null);
        currentAmmo = currentWeapon.MaxAmmo;
        updateAmmo();
    }

    private void updateAmmo()
    {
        if (data != null) data.getResourceDisplay().updateAmmoBar(((float)currentAmmo) / currentWeapon.MaxAmmo);
    }

    private void updateReload()
    {
        if (data != null) data.getResourceDisplay().updateReloadBar( 1 - (reloadTime / currentWeapon.reloadTime * ((data == null) ? 1.0f : data.reloadSpeedMultiplier)));
    }

    [PunRPC]
    private void equipGun(int index)
    {
        this.currentWeapon = guns[index];  
    }

    [PunRPC]
    private void updateItem()
    {
        spriteRenderer.sprite = currentWeapon.sprite;
    }

    public void remoteUseItem()
    {
        var vector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        var direction = Vector2.Angle(Vector2.right, new Vector2(vector.x, vector.y));
        direction = (vector.y > 0) ? direction : -direction;

        applyKnockBack(currentWeapon.weight, -vector);

        playerEvents?.onUseItem.Invoke();
        view.RPC("useItem", RpcTarget.All, direction);
        if(losesAmmo) currentAmmo -= 1;
        updateAmmo();

        StartCoroutine("cooldownTimer");
        if (currentAmmo <= 0) reload();
    }

    public void remoteUseItemWithoutDirection()
    {
        var ai = GetComponent<EnemyAi>();
        Vector2 vector = transform.right;
        if(ai != null)
        {
            var flipped = ai.getIsFlipped();
            vector = (flipped) ? -transform.right : transform.right;
        }
        
        var direction = Vector2.Angle(Vector2.right, new Vector2(vector.x, vector.y));
        direction = (vector.y > 0) ? direction : -direction;

        applyKnockBack(currentWeapon.weight, -vector);

        playerEvents?.onUseItem.Invoke();
        view.RPC("useItem", RpcTarget.All, direction);
        if (losesAmmo) currentAmmo -= 1;
        updateAmmo();

        StartCoroutine("cooldownTimer");
        if (currentAmmo <= 0) reload();
    }

    private void reload()
    {
        isReloading = true;
        currentAmmo = 0;
        StartCoroutine("reloadTimer");
    }


    public void applyKnockBack(Weight weight, Vector2 direction)
    {
        if (!gameObject.activeInHierarchy) return;
        var force = 5f;
        StopCoroutine("knockbackTimer");
        StartCoroutine("knockbackTimer", direction * ((int)weight * 1.2f) * force);
    }

    private IEnumerator knockbackTimer(Vector2 force)
    {
        var frames = 5;
        var remainingForce = force;
        while (frames > 0)
        {
            rb.position += force * Time.deltaTime;
            yield return new WaitForFixedUpdate();
            frames--;
            force *= .65f;
        }
    }

    [PunRPC]
    private void useItem(float angle)
    {
        StartCoroutine("useTimer", angle);
    }

    private IEnumerator cooldownTimer()
    {
        cooldown = currentWeapon.fireCooldown;
        while (cooldown > 0)
        {
            yield return new WaitForEndOfFrame();
            cooldown -= Time.deltaTime;
        }
    }

    private IEnumerator reloadTimer()
    {


        reloadTime = currentWeapon.reloadTime * ((data == null) ? 1.0f : data.reloadSpeedMultiplier);
        while (reloadTime > 0)
        {
            yield return new WaitForEndOfFrame();
            reloadTime -= Time.deltaTime;
            updateReload();
        }
        currentAmmo = currentWeapon.MaxAmmo;
        updateAmmo();
        isReloading = false;
    }

    private IEnumerator useTimer(float angle)
    {
        var projectiles = currentWeapon.projectiles;
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (data == null ) currentWeapon.spawnNext(i, gameObject, itemSocket, angle, 1);
            else currentWeapon.spawnNext(i, gameObject, itemSocket, angle, data.damageMultiplier);
            callUseEvent();
            setItemTrigger();
            yield return new WaitForSeconds(currentWeapon.spawnDelay);
        }
    }

    public void addAmmo(float percent)
    {
        if (isReloading) reloadTime = 0;

        currentAmmo = Mathf.Clamp(currentAmmo + Mathf.CeilToInt(currentWeapon.MaxAmmo * percent), 0, currentWeapon.MaxAmmo);
        updateAmmo();
    }

    public void setItemTrigger()
    {
        itemAnim.SetTrigger(currentWeapon.type.ToString());
    }

    private void callUseEvent()
    {
        switch (this.currentWeapon.weight)
        {
            case Weight.LIGHT:
                onUseLightItem.Invoke();
                break;
            case Weight.MEDIUM:
                onUseMediumItem.Invoke();
                break;
            case Weight.HEAVY:
                onUseHeavyItem.Invoke();
                break;
            default:
                break;
        }

    }

}
