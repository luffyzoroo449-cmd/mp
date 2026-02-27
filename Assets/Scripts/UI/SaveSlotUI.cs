using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ShadowRace.Core;

namespace ShadowRace.UI
{
    public class SaveSlotUI : MonoBehaviour
    {
        [Header("Slot Information")]
        public int slotIndex = 1; // e.g., Slot 1, 2, 3
        public TextMeshProUGUI slotTextInfo;
        public Button loadButton;
        public Button deleteButton;

        private string savePrefix;

        private void Start()
        {
            savePrefix = $"ShadowRaceSave_Slot{slotIndex}";
            RefreshSlotUI();
        }

        public void RefreshSlotUI()
        {
            if (PlayerPrefs.HasKey($"{savePrefix}_Level"))
            {
                int savedLevel = PlayerPrefs.GetInt($"{savePrefix}_Level");
                float savedXP = PlayerPrefs.GetFloat($"{savePrefix}_XP");
                
                slotTextInfo.text = $"Slot {slotIndex} - Level {savedLevel} - XP: {savedXP}";
                loadButton.interactable = true;
                deleteButton.interactable = true;
            }
            else
            {
                slotTextInfo.text = $"Slot {slotIndex} - Empty";
                // Optionally allow load button to act as "New Game" for this slot instead of disabling
                loadButton.interactable = true; 
                deleteButton.interactable = false;
            }
        }

        public void OnClickSelectSlot()
        {
            if (GameManager.Instance != null)
            {
                // Tell the GameManager which prefix to use going forward
                GameManager.Instance.saveFileName = savePrefix;

                if (PlayerPrefs.HasKey($"{savePrefix}_Level"))
                {
                    // Load Existing Game
                    GameManager.Instance.LoadGame();
                    UnityEngine.SceneManagement.SceneManager.LoadScene($"Level_{GameManager.Instance.currentLevel}");
                }
                else
                {
                    // Start New Game in this slot
                    GameManager.Instance.currentLevel = 1;
                    GameManager.Instance.playerMoney = 0;
                    GameManager.Instance.playerXP = 0;
                    GameManager.Instance.playerLevel = 1;
                    GameManager.Instance.SaveGame(); // Instantiate the initial save
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Level_1");
                }
            }
            
            // Play Audio
            if(ShadowRace.Audio.AudioManager.Instance != null)
            {
                ShadowRace.Audio.AudioManager.Instance.PlayUIClick();
            }
        }

        public void OnClickDeleteSlot()
        {
            PlayerPrefs.DeleteKey($"{savePrefix}_Level");
            PlayerPrefs.DeleteKey($"{savePrefix}_Money");
            PlayerPrefs.DeleteKey($"{savePrefix}_XP");
            PlayerPrefs.DeleteKey($"{savePrefix}_PlayerLevel");
            
            RefreshSlotUI(); // Update texts and buttons
            Debug.Log($"Deleted Save Slot {slotIndex}");
            
             // Play Audio
            if(ShadowRace.Audio.AudioManager.Instance != null)
            {
                ShadowRace.Audio.AudioManager.Instance.PlayUIClick();
            }
        }
    }
}
