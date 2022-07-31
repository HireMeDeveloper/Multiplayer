using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPiece : MonoBehaviour
{
    public Image fill;

    private Animator anim;

    private int max = 100;
    private int value;

    private float percent;

    private bool isBroken = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        value = max;
    }

    public int damage(int amount)
    {

        var savedValue = value;
        value = Mathf.Clamp(value - amount, 0, max);
        updateFill();

        if (value <= 0)
        {
            breakPiece();
            return amount - savedValue;
        }

        return 0;
    }

    public void heal(int amount)
    {
        value = Mathf.Clamp(value + amount, 0, max);
        updateFill();
    }

    public void repairPiece()
    {
        this.isBroken = false;
        anim.SetTrigger("Repair");
        heal(max);
    }

    private void breakPiece()
    {
        isBroken = true;
        anim.SetTrigger("Break");
    }

    public bool getIsBroken()
    {
        return this.isBroken;
    }

    private void updateFill()
    {
        Debug.Log("fill is now" + value / max);
        fill.rectTransform.localScale = new Vector3(value /(float) max, 1, 1);
    }

    public int getMax()
    {
        return this.max;
    }
}
