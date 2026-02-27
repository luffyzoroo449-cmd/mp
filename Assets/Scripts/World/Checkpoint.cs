using UnityEngine;
using ShadowRace.Core;

namespace ShadowRace.World
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint Visuals")]
        public SpriteRenderer glowSprite;
        public Color activeColor = Color.green;
        public Color inactiveColor = Color.white;

        private bool isActive = false;

        private void Start()
        {
            if (glowSprite != null) glowSprite.color = inactiveColor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !isActive)
            {
                ActivateCheckpoint(other.transform);
            }
        }

        private void ActivateCheckpoint(Transform playerTransform)
        {
            isActive = true;
            if (glowSprite != null) glowSprite.color = activeColor;
            
            // Register with the local LevelManager
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.SetCheckpoint(transform.position);
            }

            // Save the game state globally (XP, Money, Levels)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveGame();
            }

            Debug.Log($"Checkpoint Reached! Game Saved at {transform.position}");
            
            // Optional: Play Sound/VFX
            // AudioManager.Instance.PlaySFX(...)
        }
    }
}
