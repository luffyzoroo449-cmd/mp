using UnityEngine;
using Cinemachine;

namespace ShadowRace.Core
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }

        private CinemachineVirtualCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin perlin;

        private float shakeTimer;
        private float startingIntensity;
        private float shakeTimerTotal;

        private void Awake()
        {
            Instance = this;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }

        public void Shake(float intensity, float time)
        {
            if (perlin == null) return;

            perlin.m_AmplitudeGain = intensity;
            startingIntensity = intensity;
            shakeTimerTotal = time;
            shakeTimer = time;
        }

        private void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0f)
                {
                    perlin.m_AmplitudeGain = 0f;
                }
                else
                {
                    // Linear fade out
                    perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
                }
            }
        }
    }
}
