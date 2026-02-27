using UnityEngine;

namespace ShadowRace.Audio
{
    public class SurfaceAudioCaster : MonoBehaviour
    {
        [Header("Raycast Settings")]
        public Transform footPosition;
        public float rayDistance = 0.5f;
        public LayerMask groundLayer;

        [Header("Audio Settings")]
        [Tooltip("Assign generic clip if surface is unknown.")]
        public AudioClip defaultFootstep;

        // Note: In a full AAA game, we would use FMOD or Wwise to handle this seamlessly.
        // For standard Unity audio, we trigger this from an Animation Event.
        
        public void PlayFootstep()
        {
            if (footPosition == null) return;

            RaycastHit2D hit = Physics2D.Raycast(footPosition.position, Vector2.down, rayDistance, groundLayer);

            if (hit.collider != null)
            {
                string surfaceTag = hit.collider.tag;

                // Example logic to trigger localized audio
                // AudioManager.Instance.PlaySurfaceAudio(surfaceTag, footPosition.position);
                
                // Debug log to verify detection
                // Debug.Log("Stepped on surface: " + surfaceTag);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (footPosition != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(footPosition.position, footPosition.position + Vector3.down * rayDistance);
            }
        }
    }
}
