using UnityEngine;

namespace ShadowRace.Audio
{
    public class BGMTrigger : MonoBehaviour
    {
        [Header("Region Settings")]
        [Tooltip("The audio clip to play when the player enters this zone.")]
        public AudioClip regionTheme;
        public bool playOnStart;

        private void Start()
        {
            if (playOnStart && regionTheme != null)
            {
                TriggerMusic();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                TriggerMusic();
            }
        }

        private void TriggerMusic()
        {
            if (AudioManager.Instance != null && regionTheme != null)
            {
                AudioManager.Instance.PlayBGM(regionTheme);
                Debug.Log($"BGM changed to {regionTheme.name}");
            }
        }
    }
}
