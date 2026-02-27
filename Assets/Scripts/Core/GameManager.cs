using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowRace.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Global Game State")]
        public int currentLevel = 1; // Out of 75
        public int playerMoney = 0;
        public float playerXP = 0f;
        public int playerLevel = 1;
        
        [Header("Settings")]
        public string saveFileName = "ShadowRaceSave";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadGame();
        }

        public void CompleteLevel()
        {
            Debug.Log($"Level {currentLevel} Completed!");
            currentLevel++;

            if (currentLevel > 75)
            {
                Debug.Log("Game Defeated! Final Boss Down.");
                // Transition to credits
                return;
            }

            SaveGame();
            LoadNextLevelScene();
        }

        private void LoadNextLevelScene()
        {
            // Assuming scenes are named "Level_1", "Level_2" etc.
            string nextSceneName = $"Level_{currentLevel}";
            Debug.Log($"Loading Scene: {nextSceneName}");
            // SceneManager.LoadScene(nextSceneName);
        }

        public void SaveGame()
        {
            PlayerPrefs.SetInt($"{saveFileName}_Level", currentLevel);
            PlayerPrefs.SetInt($"{saveFileName}_Money", playerMoney);
            PlayerPrefs.SetFloat($"{saveFileName}_XP", playerXP);
            PlayerPrefs.SetInt($"{saveFileName}_PlayerLevel", playerLevel);
            PlayerPrefs.Save();
            Debug.Log("Game Saved.");
        }

        public void LoadGame()
        {
            if (PlayerPrefs.HasKey($"{saveFileName}_Level"))
            {
                currentLevel = PlayerPrefs.GetInt($"{saveFileName}_Level");
                playerMoney = PlayerPrefs.GetInt($"{saveFileName}_Money");
                playerXP = PlayerPrefs.GetFloat($"{saveFileName}_XP");
                playerLevel = PlayerPrefs.GetInt($"{saveFileName}_PlayerLevel");
                Debug.Log("Game Loaded.");
            }
            else
            {
                Debug.Log("No Save Found. Starting New Game.");
            }
        }
    }
}
