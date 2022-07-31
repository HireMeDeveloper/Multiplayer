using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void connectCamera(GameObject playerObject)
    {
        virtualCamera.Follow = playerObject.transform;

        var shaker = GetComponent<ScreenShake>();
        var user = playerObject.GetComponent<ItemUser>();
        var data = playerObject.GetComponent<PlayerData>();

        if (shaker == null || user == null || data == null) return;

        data.onKill.AddListener(() => shaker.mediumShake());
        //user.onUseLightItem.AddListener(() => shaker.lightShake());
        //user.onUseMediumItem.AddListener(() => shaker.mediumShake());
        //user.onUseHeavyItem.AddListener(() => shaker.heavyShake());
    }
}
