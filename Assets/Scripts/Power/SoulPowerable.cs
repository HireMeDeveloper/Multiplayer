using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoulPowerable : MonoBehaviour
{
    [SerializeField] private int soulsNeeded = 10;
    private int soulsRemaining;

    public UnityEvent onAddSoul;
    public UnityEvent onCompleteSoulPower;

    

    private void Start()
    {
        soulsRemaining = soulsNeeded;
    }

    public void addSoul()
    {
        if (soulsRemaining == 0) return;
        soulsRemaining -= 1;
        onAddSoul.Invoke();

        if(soulsRemaining <= 0)
        {
            onCompleteSoulPower.Invoke();
        }
    }

    public bool AcceptsSouls()
    {
        return soulsRemaining > 0;
    }

}
