using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    public Image healthFill;
    public Image ammoFill;

    [SerializeField] private Color blue;
    [SerializeField] private Color green;
    public void updateHealthBar(float scale)
    {
        healthFill.transform.localScale = new Vector3(scale, 1, 1);
    }
    public void updateAmmoBar(float scale)
    {
        ammoFill.color = blue;
        ammoFill.transform.localScale = new Vector3(scale, 1, 1);
    }

    public void updateReloadBar(float scale)
    {
        ammoFill.color = green;
        ammoFill.transform.localScale = new Vector3(scale, 1, 1);
    }
}
