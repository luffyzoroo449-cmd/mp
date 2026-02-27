using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming using TextMeshPro for UI
using ShadowRace.Player;

namespace ShadowRace.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        public PlayerStats playerStats; // Drag the player here in inspector

        [Header("Bars")]
        public Image hpBarFill;
        public Image mpBarFill;
        public Image staminaBarFill;
        public Image xpBarFill;

        [Header("Text Elements")]
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI moneyText;

        private void Update()
        {
            if (playerStats == null) return;

            UpdateBars();
            UpdateTextElements();
        }

        private void UpdateBars()
        {
            if (hpBarFill != null && playerStats.maxHP > 0)
                hpBarFill.fillAmount = playerStats.currentHP / playerStats.maxHP;

            if (mpBarFill != null && playerStats.maxMP > 0)
                mpBarFill.fillAmount = playerStats.currentMP / playerStats.maxMP;

            if (staminaBarFill != null && playerStats.maxStamina > 0)
                staminaBarFill.fillAmount = playerStats.currentStamina / playerStats.maxStamina;

            if (xpBarFill != null && playerStats.xpToNextLevel > 0)
                xpBarFill.fillAmount = playerStats.currentXP / playerStats.xpToNextLevel;
        }

        private void UpdateTextElements()
        {
            if (levelText != null)
                levelText.text = $"Lv {playerStats.level}";

            if (moneyText != null)
                moneyText.text = $"{playerStats.money}";
        }
    }
}
