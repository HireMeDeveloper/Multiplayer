using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PropertiesManager : MonoBehaviour
{
    public static PropertiesManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;

        if (instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
        
    }

    public void setPlayerProperty(Player player, string name, int value)
    {
        var properties = player.CustomProperties;
        if (properties.ContainsKey(name))
        {
            properties[name] = value;
        } else
        {
            properties.Add(name, value);
        }

        player.SetCustomProperties(properties);
    }
}
