using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public HealthPiece pieceRef;
    public Transform peiceParent;

    private List<HealthPiece> pieces = new List<HealthPiece>();

    private int maxHp;

    private void Awake()
    {
        updateDisplay(3);
    }

    public void repairAll()
    {
        foreach (var hpPiece in pieces)
        {
            hpPiece.repairPiece();
        }
    }

    public void damage(int amount)
    {
        var amountToDamage = amount;
        for (int i = pieces.Count - 1; i >= 0; i--)
        {
            if (pieces[i].getIsBroken()) continue;

            amountToDamage = pieces[i].damage(amountToDamage);
        }
    }

    public void heal(int amount)
    {
        nextPiece().heal(amount);
    }

    private HealthPiece nextPiece()
    {
        for (int i = pieces.Count - 1; i >= 0; i--)
        {
            if (pieces[i].getIsBroken()) continue;
            return pieces[i];
        }
        return pieces[0];
    }

    public void updateDisplay(int count)
    {
        foreach (var hpPiece in pieces)
        {
            Destroy(hpPiece.gameObject);
        }

        pieces.Clear();

        for (int i = 0; i < count; i++)
        {
            pieces.Add(Instantiate(pieceRef, peiceParent));
        }

        updateValues();
    }

    private void updateValues()
    {
        if (pieces.Count == 0) return;

        maxHp = pieces[0].getMax() * pieces.Count;
    }
}
