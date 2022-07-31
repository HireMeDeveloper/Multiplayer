using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RegionManager : MonoBehaviour
{
    public Dropdown regionDropdown;

    public PhotonRegion[] regions;

    private void Start()
    {
        GetRegions();
    }

    void GetRegions()
    {
        //regionDropdown.options.Clear();
        foreach (PhotonRegion region in regions)
        {
            regionDropdown.options.Add(new Dropdown.OptionData(region.token.ToString()));
        }
        regionDropdown.value = 0;
        regionDropdown.captionText.text = regionDropdown.options[0].text;
    }

    public void PickRegion()
    {
        PhotonNetwork.Disconnect();
        string regionSwitchCode = regionDropdown.options[regionDropdown.value].text;
        PhotonNetwork.ConnectToRegion(regionSwitchCode);
    }
}
