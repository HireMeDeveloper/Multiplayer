using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings/SettingsDataSO")]
public class SettingsData : ScriptableObject
{
    public bool isOnline = true;

    public KeyCode shootButton = KeyCode.Mouse0;
    public KeyCode dodgeButton = KeyCode.Space;
    public KeyCode interactButton = KeyCode.F;
    public KeyCode reloadButton = KeyCode.R;

    public float maxVolume = 1f;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambientVolume = 1f;
    public float voiceVolume = 1f;

}
