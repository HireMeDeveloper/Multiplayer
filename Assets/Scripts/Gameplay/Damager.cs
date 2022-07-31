using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damager : Spawnable
{
    [SerializeField] private List<string> ignoreTags = new List<string>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionDamage(collision);
    }

    public void damageInside()
    {
        var collider = GetComponent<BoxCollider2D>();
        Collider2D[] results = new Collider2D[20];
        collider.OverlapCollider(new ContactFilter2D(), results);

        foreach (var collision in results)
        {
            collisionDamage(collision);
        }
    }

    private void collisionDamage(Collider2D collision)
    {
        if (collision == null) return;
        if (ignoreTags.Contains(collision.gameObject.tag)) return;
        var damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable == null) return;

        lifetime -= pierceReduction;

        var data = ((GameObject)shooter.TagObject).GetComponent<PlayerData>();

        damageable.damage((data != null) ? Mathf.FloorToInt(damage * data.damageMultiplier) : damage, shooter);

        var itemUser = collision.gameObject.GetComponent<ItemUser>();

        if (itemUser == null) return;

        var direction = collision.gameObject.transform.position - transform.position;

        if (collision.gameObject.tag == "Player") return;
        itemUser.applyKnockBack(Weight.LIGHT, direction);
    }
}
