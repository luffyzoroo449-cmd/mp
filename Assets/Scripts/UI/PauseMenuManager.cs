using UnityEngine;
using UnityEngine.SceneManagement;
using ShadowRace.Audio;

namespace ShadowRace.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject pauseMenuUI;

        private bool isPaused = false;

        private void Update()
        {
            // Simple toggle map - recommend integrating into PlayerInputHandler New Input System
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused) Resume();
                else Pause();
            }
        }

        public void Resume()
        {
            PlayClickSound();
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // Unfreeze time
            isPaused = false;
        }

        private void Pause()
        {
            PlayClickSound();
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f; // Freeze game Time
            isPaused = true;
        }

        public void LoadMenu()
        {
            PlayClickSound();
            Time.timeScale = 1f; // Ensure time is unpaused before loading
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            PlayClickSound();
            Debug.Log("Quitting Game...");
            Application.Quit();
        }

        private void PlayClickSound()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayUIClick();
            }
        }
    }
}
