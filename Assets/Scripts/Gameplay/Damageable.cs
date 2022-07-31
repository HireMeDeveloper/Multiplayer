using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public abstract class Damageable : MonoBehaviour
{
    public UnityEvent onDamage;
    public UnityEvent<Vector2> onDeath;

    public virtual void damage(int damage, Player damager)
    {
        onDamage.Invoke();
    }

    [PunRPC]
    public void damageRPC()
    {
        onDamage.Invoke();
    }

    [PunRPC]
    public virtual void death()
    {
        var pos = new Vector2(transform.position.x, transform.position.y);
        onDeath.Invoke(pos);
    }

}
