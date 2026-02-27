using UnityEngine;

namespace ShadowRace.World
{
    public class InteriorZone : MonoBehaviour
    {
        [Header("Visuals")]
        [Tooltip("The SpriteRenderer (e.g., Roof) to fade out when the player enters.")]
        public SpriteRenderer roofSprite;
        public float fadeSpeed = 3f;

        private bool isPlayerInside = false;

        private void Update()
        {
            if (roofSprite != null)
            {
                Color c = roofSprite.color;
                float targetAlpha = isPlayerInside ? 0f : 1f;
                c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
                roofSprite.color = c;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = true;
                // Optional: Muffle outdoor audio via AudioManager
                if (ShadowRace.Audio.AudioManager.Instance != null)
                {
                    // ShadowRace.Audio.AudioManager.Instance.SetIndoorAudioProfile(true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = false;
                if (ShadowRace.Audio.AudioManager.Instance != null)
                {
                    // ShadowRace.Audio.AudioManager.Instance.SetIndoorAudioProfile(false);
                }
            }
        }
    }
}
