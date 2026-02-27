using UnityEngine;
using UnityEngine.SceneManagement;
using ShadowRace.Core;

namespace ShadowRace.UI
{
    public class GameOverManager : MonoBehaviour
    {
        [Header("UI Reference")]
        public GameObject gameOverPanel;
        
        [Header("Settings")]
        public float delayBeforeShow = 1.5f;

        private void OnEnable()
        {
            // Ideally, PlayerStats would fire an event when HP <= 0.
            // For this architecture, we can assume PlayerStats or GameManager calls this directly.
        }

        public void TriggerGameOver()
        {
            StartCoroutine(ShowGameOverRoutine());
        }

        private System.Collections.IEnumerator ShowGameOverRoutine()
        {
            yield return new WaitForSeconds(delayBeforeShow);
            
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the background action

            // Optional: Play Game Over Stinger Audio
            if (ShadowRace.Audio.AudioManager.Instance != null)
            {
                // ShadowRace.Audio.AudioManager.Instance.PlaySFX(gameOverClip);
            }
        }

        public void OnClickRespawn()
        {
            Time.timeScale = 1f;
            
            if (LevelManager.Instance != null && LevelManager.Instance.HasCheckpoint())
            {
                // Reload current scene to reset enemies, then move player to checkpoint
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                // No checkpoint reached, restart level entirely
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void OnClickQuitToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
