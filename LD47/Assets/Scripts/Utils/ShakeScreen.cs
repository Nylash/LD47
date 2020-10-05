using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeScreen : MonoBehaviour
{
    public static ShakeScreen instance;

    [SerializeField] float shakeDuration = .5f;
    [SerializeField] float shakeAmplitude = .3f;
    [SerializeField] float shakeFrequency = .3f;

    Coroutine shakeCoroutine;
    public CinemachineBasicMultiChannelPerlin VCamNoise;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        VCamNoise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        if (shakeCoroutine == null)
            shakeCoroutine = StartCoroutine(DoShakeScreen());
    }

    IEnumerator DoShakeScreen()
    {
        VCamNoise.m_AmplitudeGain = shakeAmplitude;
        VCamNoise.m_FrequencyGain = shakeFrequency;
        yield return new WaitForSeconds(shakeDuration);
        VCamNoise.m_AmplitudeGain = 0;
        yield return new WaitForSeconds(shakeDuration);
        shakeCoroutine = null;
    }
}
