using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Spawnable : MonoBehaviour
{

    public Vector2 initialVelocity;
    protected int damage;
    protected float lifetime;

    protected float pierceReduction;

    protected Rigidbody2D rb;

    protected Player shooter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator projectileTimer()
    {
        while (lifetime > 0)
        {
            yield return new WaitForEndOfFrame();
            lifetime -= Time.deltaTime;
        }
        remove();
    }

    private void Start()
    {
        if (rb != null) rb.velocity = rb.transform.right * initialVelocity.x + rb.transform.up * initialVelocity.y;
    }

    public void setupSpawnable(Vector2 initialVelocity, float lifetime, int damage, float pierceReduction, Player player)
    {
        this.pierceReduction = pierceReduction;
        this.initialVelocity = initialVelocity;
        this.damage = damage;
        this.lifetime = lifetime;
        this.shooter = player;
        StartCoroutine("projectileTimer");
    }

    protected void remove()
    {
        Destroy(this.gameObject);
    }
}
    

