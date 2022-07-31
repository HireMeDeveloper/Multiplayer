using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private float maxVelocity = 5f;

    [Header("Spawnables")]
    [SerializeField] private GameObject[] spawnables;
    [SerializeField] private int rollChance = 50;

    public void spawnAsLoot(int rolls)
    {
        for (int i = 0; i < rolls; i++)
        {
            var rand = Random.Range(1, 101);
            if (rand <= rollChance) spawnRandom(1);
        }
    }

    public void spawnRandom(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnWithRandomVelocity(chooseRandomObject());
        }
    }

    private GameObject spawn(GameObject spawnable)
    {
        return Instantiate(spawnable, spawnTransform.position, Quaternion.identity);
    }

    private void spawnWithRandomVelocity(GameObject spawnable)
    {
        var spawned = spawn(spawnable);

        var rb = spawned.GetComponent<Rigidbody2D>();

        if (rb == null) return;

        rb.velocity = new Vector2(Random.Range(-maxVelocity, maxVelocity), Random.Range(-maxVelocity, maxVelocity));
    }

    private GameObject chooseRandomObject()
    {
        if (spawnables.Length == 0) return null;

        var index = Random.Range(0, spawnables.Length);

        return spawnables[index];
    }
}
