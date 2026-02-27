using System.Collections;
using UnityEngine;

namespace ShadowRace.Core
{
    public class HitStopManager : MonoBehaviour
    {
        public static HitStopManager Instance { get; private set; }

        private bool isWaiting;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void StopTime(float duration, float timeScale = 0.05f)
        {
            if (isWaiting) return;
            
            Time.timeScale = timeScale;
            StartCoroutine(WaitSystem(duration));
        }

        private IEnumerator WaitSystem(float duration)
        {
            isWaiting = true;
            // Use real time so it's not affected by the slomo
            yield return new WaitForSecondsRealtime(duration);
            
            Time.timeScale = 1.0f;
            isWaiting = false;
        }
        
        public static void PlayImpactVFX(GameObject particlePrefab, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            if (particlePrefab != null)
            {
                GameObject vfx = Instantiate(particlePrefab, spawnPosition, spawnRotation);
                // Assume particle system is set to auto-destroy or destroy via script
                Destroy(vfx, 2f); // Fallback cleanup
            }
        }
    }
}
