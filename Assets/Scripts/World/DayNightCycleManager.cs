using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

namespace ShadowRace.World
{
    public class DayNightCycleManager : MonoBehaviour
    {
        public static DayNightCycleManager Instance { get; private set; }

        [Header("Time Settings")]
        [Tooltip("Current time in hours (0-24)")]
        [Range(0f, 24f)]
        public float timeOfDay = 8f; // Start at 8 AM
        public float timeScale = 1f; // How fast time passes

        [Header("Lighting")]
        public Light2D globalLight;
        public Gradient lightColorCurve;
        public AnimationCurve lightIntensityCurve;

        public event Action<float> OnTimeChanged;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            timeOfDay += Time.deltaTime * timeScale;
            if (timeOfDay >= 24f)
            {
                timeOfDay %= 24f; // Loop back to 0
            }

            UpdateLighting();
            OnTimeChanged?.Invoke(timeOfDay);
        }

        private void UpdateLighting()
        {
            if (globalLight != null)
            {
                float normalizedTime = timeOfDay / 24f;
                globalLight.color = lightColorCurve.Evaluate(normalizedTime);
                globalLight.intensity = lightIntensityCurve.Evaluate(normalizedTime);
            }
        }

        public bool IsNight()
        {
            return timeOfDay >= 19f || timeOfDay <= 5f;
        }
    }
}
