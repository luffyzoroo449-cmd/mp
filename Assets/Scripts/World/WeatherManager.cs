using UnityEngine;

namespace ShadowRace.World
{
    public class WeatherManager : MonoBehaviour
    {
        public static WeatherManager Instance { get; private set; }

        [Header("Particle Systems")]
        public ParticleSystem rainParticles;
        public ParticleSystem snowParticles;
        public ParticleSystem ashParticles;

        public enum WeatherType { Clear, Rain, Snow, Ash }
        public WeatherType CurrentWeather { get; private set; } = WeatherType.Clear;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            SetWeather("clear"); // Initialize to clear weather
        }

        public void SetWeather(string weatherType)
        {
            // Turn everything off first
            if (rainParticles != null) rainParticles.Stop();
            if (snowParticles != null) snowParticles.Stop();
            if (ashParticles != null) ashParticles.Stop();
            {
                newWeather.Play();
                activeWeather = newWeather;
                Debug.Log($"Weather changed to {newWeather.gameObject.name}");
            }
        }
    }
}
