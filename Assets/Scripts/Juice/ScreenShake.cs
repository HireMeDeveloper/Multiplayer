using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    private new CinemachineVirtualCamera camera;
    private float shakeTime;

    private void Awake()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void lightShake()
    {
        StopAllCoroutines();
        ShakeCamera(.4f * 2, 0.1f);
    }

    public void mediumShake()
    {
        StopAllCoroutines();
        ShakeCamera(0.8f * 2, 0.11f);
    }

    public void heavyShake()
    {
        StopAllCoroutines();
        ShakeCamera(1.6f * 2, 0.12f);
    }

    public void ShakeCamera(float intensity, float time)
    {
        var perlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        shakeTime = time;
        StartCoroutine("shakeTimer");
    }

    private IEnumerator shakeTimer()
    {
        while(shakeTime > 0)
        {
            yield return new WaitForEndOfFrame();
            shakeTime -= Time.deltaTime;
        }
        var perlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0;
    }
}
