using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UiRefrenceManager : MonoBehaviour
{
    [SerializeField] private Text interactionText;
    [SerializeField] private GameObject interactionGameobject;

    [SerializeField] private Transform perkParent;
    [SerializeField] private PerkIcon perkIcon;

    private List<PerkIcon> perkIcons = new List<PerkIcon>();
    public void showInteractionText(string text)
    {
        interactionText.text = text;
        interactionGameobject.SetActive(true);
    }

    public void hideInteractionText()
    {
        interactionGameobject.SetActive(false);
    }

    public void addPerkIcon(PerkData perkData)
    {
        if (hasPerk(perkData)) return;

        var icon = Instantiate(perkIcon, perkParent);
        icon.SetPerk(perkData);
        perkIcons.Add(icon);
    }

    public void upgradePerkIcon(PerkData perkData)
    {
        if (!hasPerk(perkData)) return;

        var icon = getIcon(perkData);
        icon.UpdateIcon();
    }

    public void clearPerkIcons()
    {
        foreach (var icon in perkIcons)
        {
            Destroy(icon.gameObject);
        }

        perkIcons.Clear();
    }

    private bool hasPerk(PerkData perkData)
    {
        return perkIcons
            .Select((icon) => icon.GetPerk())
            .Where((data) => data == perkData)
            .ToList().Count > 0;
    }

    private PerkIcon getIcon(PerkData perkData)
    {
        if (!hasPerk(perkData)) return null;

        return perkIcons
            .Where((icon) => icon.GetPerk() == perkData)
            .ToList().First();
    }
}
