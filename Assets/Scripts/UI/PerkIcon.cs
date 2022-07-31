using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkIcon : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private PerkData perk;
    private int level = 1;

    public void SetPerk(PerkData perk)
    {
        this.perk = perk;
        UpdateIcon();
    }

    public void Upgrade()
    {
        level = 2;
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        image.sprite = (level == 1) ? perk.perkIcon : perk.proPerkIcon;
    }

    public int GetLevel()
    {
        return level;
    }

    public PerkData GetPerk()
    {
        return this.perk;
    }

    private void OnValidate()
    {
        UpdateIcon();
    }

}
