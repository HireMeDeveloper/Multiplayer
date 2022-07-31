using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[CreateAssetMenu(menuName = "Game/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Display")]
    public Sprite sprite;
    public float xOffset;
    public ItemType type;

    [Header("Projectiles")]
    public List<Spawnable> projectiles;
    public float spread;
    public float spawnDelay;
    public float pierceReduction = 0;

    [Header("Stats")]
    public Vector2 initialVelocity;
    public int damagePerProjectile;
    public float projectileLifetime;
    public float fireCooldown = 0.5f;
    public float reloadTime = 1.5f;
    public int MaxAmmo;
    public Weight weight;
    public bool fullAuto = false;

    [Header("Info")]
    public WeaponType weaponType;
    public int price;

    public void spawnNext(int i, GameObject user, GameObject socket, float direction, float damageMultiplier)
    {
        var rotation = user.transform.rotation;
        var scale = user.transform.localScale.x;

        Spawnable prefabObject = null;
        var vector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - user.transform.position).normalized;

        if (projectiles.Count == 1)
        {
            prefabObject = Instantiate(projectiles[i], socket.transform.position, Quaternion.Euler(rotation.x, rotation.y, direction));
        }
        else if (projectiles.Count == 2)
        {
            var lockedRotation = (i == 0) ? -spread : spread;
            prefabObject = Instantiate(projectiles[i], socket.transform.position, Quaternion.Euler(rotation.x, rotation.y, lockedRotation + direction));
        }
        else if (projectiles.Count > 2)
        {
            var delta = (2 * spread) / projectiles.Count;
            prefabObject = Instantiate(projectiles[i], socket.transform.position, Quaternion.Euler(rotation.x, rotation.y, (-spread + (delta * i)) + direction));
        }

        var prefabScale = prefabObject.transform.localScale;

        prefabObject.transform.localScale = new Vector3(prefabScale.x, prefabScale.y * scale, prefabScale.z);
        

        var spawnable = prefabObject?.GetComponent<Spawnable>();

        var view = user.GetPhotonView();
        var player = view.Owner;

        if (player == null) Debug.Log("shooter let to null player");

        if (spawnable != null) spawnable.setupSpawnable(initialVelocity, projectileLifetime, Mathf.CeilToInt(damagePerProjectile * damageMultiplier), pierceReduction, player);
    }

    
}
