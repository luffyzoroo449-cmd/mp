using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ShadowRace.Core;
using ShadowRace.Audio;

namespace ShadowRace.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject mainPanel;
        public GameObject optionsPanel;

        [Header("Buttons")]
        public Button continueButton;

        private void Start()
        {
            // Play Menu Theme
            if (AudioManager.Instance != null && AudioManager.Instance.villageTheme != null)
            {
                AudioManager.Instance.PlayBGM(AudioManager.Instance.villageTheme);
            }

            // Check if there is a save file to enable the Continue button
            if (PlayerPrefs.HasKey(GameManager.Instance != null ? $"{GameManager.Instance.saveFileName}_Level" : "ShadowRaceSave_Level"))
            {
                if(continueButton != null) continueButton.interactable = true;
            }
            else
            {
                if(continueButton != null) continueButton.interactable = false;
            }
        }

        public void OnClickNewGame()
        {
            PlayClickSound();
            
            // Clear old saves
            string saveName = GameManager.Instance != null ? GameManager.Instance.saveFileName : "ShadowRaceSave";
            PlayerPrefs.DeleteKey($"{saveName}_Level");
            PlayerPrefs.DeleteKey($"{saveName}_Money");
            PlayerPrefs.DeleteKey($"{saveName}_XP");
            PlayerPrefs.DeleteKey($"{saveName}_PlayerLevel");

            // Reset GameManager stats and load Level 1
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentLevel = 1;
                GameManager.Instance.playerMoney = 0;
                GameManager.Instance.playerXP = 0;
                GameManager.Instance.playerLevel = 1;
            }

            SceneManager.LoadScene("Level_1");
        }

        public void OnClickContinue()
        {
            PlayClickSound();
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadGame();
                SceneManager.LoadScene($"Level_{GameManager.Instance.currentLevel}");
            }
            else
            {
                // Fallback direct load
                int savedLevel = PlayerPrefs.GetInt("ShadowRaceSave_Level", 1);
                SceneManager.LoadScene($"Level_{savedLevel}");
            }
        }

        public void OnClickOptions()
        {
            PlayClickSound();
            mainPanel.SetActive(false);
            optionsPanel.SetActive(true);
        }

        public void OnClickBackToMain()
        {
            PlayClickSound();
            optionsPanel.SetActive(false);
            mainPanel.SetActive(true);
        }

        public void OnClickQuit()
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
