using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private SettingsData settingsData;
    public void show()
    {
        settingsMenu.SetActive(true);
    }
    public void hide()
    {
        settingsMenu.SetActive(false);
    }

    public void increaseVolume(AudioType type)
    {

    }

    public void decreaseVolume(AudioType type)
    {

    }
}
