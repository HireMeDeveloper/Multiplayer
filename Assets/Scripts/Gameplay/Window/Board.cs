using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Animator anim;

    private bool isBroken = false;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void DamageBoard()
    {
        anim.speed = 1;
        anim.SetTrigger("Damage");
    }

    public void BreakBoard()
    {
        anim.speed = 1;
        anim.SetTrigger("Break");
    }

    public void RepairBoard()
    {
        anim.speed = Random.Range(0.25f, 1.75f);
        anim.SetTrigger("Repair");
    }
}
