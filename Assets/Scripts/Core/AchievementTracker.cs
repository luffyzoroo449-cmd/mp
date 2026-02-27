using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ShadowRace.Core
{
    public class AchievementTracker : MonoBehaviour
    {
        public static AchievementTracker Instance { get; private set; }

        [System.Serializable]
        public class Achievement
        {
            public string id;
            public string title;
            public string description;
            public bool isUnlocked;
        }

        [Header("Achievement Database")]
        public List<Achievement> achievements;

        [Header("UI Popups")]
        public GameObject achievementPopupPanel;
        public TextMeshProUGUI popupTitleText;
        public TextMeshProUGUI popupDescText;
        public float popupDuration = 3f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadAchievements();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadAchievements()
        {
            foreach (var ach in achievements)
            {
                ach.isUnlocked = PlayerPrefs.GetInt("Ach_" + ach.id, 0) == 1;
            }
        }

        public void UnlockAchievement(string achievementID)
        {
            Achievement ach = achievements.Find(x => x.id == achievementID);
            
            if (ach != null && !ach.isUnlocked)
            {
                ach.isUnlocked = true;
                PlayerPrefs.SetInt("Ach_" + ach.id, 1);
                
                Debug.Log($"ACHIEVEMENT UNLOCKED: {ach.title}");
                ShowPopup(ach);
            }
        }

        private void ShowPopup(Achievement ach)
        {
            if (achievementPopupPanel != null)
            {
                popupTitleText.text = "Achievement Unlocked!";
                popupDescText.text = ach.title;
                
                achievementPopupPanel.SetActive(true);
                
                if (ShadowRace.Audio.AudioManager.Instance != null)
                {
                    // ShadowRace.Audio.AudioManager.Instance.PlaySFX(achievementSound);
                }

                CancelInvoke("HidePopup");
                Invoke("HidePopup", popupDuration);
            }
        }

        private void HidePopup()
        {
            if (achievementPopupPanel != null)
            {
                achievementPopupPanel.SetActive(false);
            }
        }
    }
}
