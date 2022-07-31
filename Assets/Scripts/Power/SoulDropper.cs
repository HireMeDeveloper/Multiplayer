using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDropper : MonoBehaviour
{
    [SerializeField] private Floater soul;

    public void SpawnSoul()
    {
        Instantiate(soul, transform.position, Quaternion.identity);
    }
}
